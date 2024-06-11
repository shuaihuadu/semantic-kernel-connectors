namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama text generation service.
/// </summary>
public sealed class OllamaTextGenerationService : OllamaTextBaseService, ITextGenerationService
{
    private readonly OllamaClient _ollamaClient;

    private Dictionary<string, object?> AttributesInternal { get; } = [];

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextGenerationService(string model, Uri endpoint, ILoggerFactory? loggerFactory = null) : base(model, endpoint, loggerFactory)
    {
        this._ollamaClient = new OllamaClient(endpoint, loggerFactory);

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextGenerationService(string model, string endpoint, ILoggerFactory? loggerFactory = null) : this(model, new Uri(endpoint), loggerFactory) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="httpClient">HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextGenerationService(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null) : base(model, httpClient, loggerFactory)
    {
        this._ollamaClient = new OllamaClient(httpClient, loggerFactory);

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, prompt, ollamaPromptExecutionSettings);

        StreamingResponse<GenerateCompletionResponse> response;

        try
        {
            response = await this._ollamaClient.GenerateCompletionStreamingAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        ConfiguredCancelableAsyncEnumerable<GenerateCompletionResponse>.Enumerator responseEnumerator = response.ConfigureAwait(false).GetAsyncEnumerator();

        List<StreamingTextContent>? streamedContents = activity is not null ? [] : null;

        try
        {
            while (true)
            {
                try
                {
                    if (!await responseEnumerator.MoveNextAsync())
                    {
                        break;
                    }
                }
                catch (Exception ex) when (activity is not null)
                {
                    activity.SetError(ex);
                    throw;
                }

                GenerateCompletionResponse currentResponse = responseEnumerator.Current;

                StreamingTextContent content = GetStreamingTextContentFromResponse(currentResponse);

                streamedContents?.Add(content);

                yield return content;
            }
        }
        finally
        {
            activity?.EndStreaming(streamedContents);
            await responseEnumerator.DisposeAsync();
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, prompt, ollamaPromptExecutionSettings);

        GenerateCompletionResponse response;

        try
        {
            response = await this._ollamaClient.GenerateCompletionAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        TextContent content = GetTextContentFromResponse(response);

        activity?.SetCompletionResponse([content]);

        return [content];
    }
}
