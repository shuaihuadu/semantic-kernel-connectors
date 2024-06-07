namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama text generation service.
/// </summary>
public sealed class OllamaTextGenerationService : ITextGenerationService
{
    private const string ModelProvider = "ollama";
    private Dictionary<string, object?> AttributesInternal { get; } = [];

    private readonly Uri? _endpoint;
    private readonly HttpClient? _httpClient;
    private readonly string _model;
    private readonly ILoggerFactory? _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted</param>
    /// <param name="httpClient">Optional HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextGenerationService(
        string model,
        Uri? endpoint = null,
        HttpClient? httpClient = null,
        ILoggerFactory? loggerFactory = null)
    {

        Verify.NotNullOrWhiteSpace(model, nameof(model));

        Verify.ValidateHttpClientAndEndpoint(httpClient, endpoint);

        this._model = model;
        this._httpClient = httpClient;
        this._loggerFactory = loggerFactory;
        this._endpoint = OllamaClientBuilder.GetOllamaClientEndpoint(httpClient, endpoint);

        this.AddAttribute(AIServiceExtensions.ModelIdKey, model);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="httpClient">Optional HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextGenerationService(
        string model,
        string? endpoint = null,
        HttpClient? httpClient = null,
        ILoggerFactory? loggerFactory = null) : this(model, string.IsNullOrWhiteSpace(endpoint) ? null : new Uri(endpoint), httpClient, loggerFactory)
    {
    }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    /// <inheritdoc />
    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, prompt, ollamaPromptExecutionSettings);

        StreamingResponse<GenerateCompletionResponse> response;

        try
        {
            using OllamaClient client = OllamaClientBuilder.CreateOllamaClient(this._httpClient, this._endpoint, this._loggerFactory);

            response = await client.GenerateCompletionStreamingAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings), cancellationToken).ConfigureAwait(false);
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

                StreamingTextContent content = GetStreamingTextContentFromStreamingResponse(currentResponse);

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
            using OllamaClient client = OllamaClientBuilder.CreateOllamaClient(this._httpClient, this._endpoint, this._loggerFactory);

            response = await client.GenerateCompletionAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings), cancellationToken);
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

    private static GenerateCompletionOptions CreateGenerateCompletionOptions(string model, string prompt, OllamaPromptExecutionSettings ollamaPromptExecutionSettings)
    {
        return new GenerateCompletionOptions
        {
            Model = model,
            Prompt = prompt,
            Format = ollamaPromptExecutionSettings.Format,
            KeepAlive = ollamaPromptExecutionSettings.KeepAlive,
            System = ollamaPromptExecutionSettings.SystemPrompt,
            Options = new ParameterOptions
            {
                NumCtx = ollamaPromptExecutionSettings.MaxTokens,
                FrequencyPenalty = ollamaPromptExecutionSettings.FrequencyPenalty,
                PresencePenalty = ollamaPromptExecutionSettings.PresencePenalty,
                Temperature = ollamaPromptExecutionSettings.Temperature,
                Seed = (int)ollamaPromptExecutionSettings.Seed,
                Stop = ollamaPromptExecutionSettings.Stop?.ToArray(),
                TopK = ollamaPromptExecutionSettings.TopK,
                TopP = ollamaPromptExecutionSettings.TopP
            }
        };
    }

    internal void AddAttribute(string key, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            this.AttributesInternal.Add(key, value);
        }
    }

    private static StreamingTextContent GetStreamingTextContentFromStreamingResponse(GenerateCompletionResponse response) => new(text: response.Response, modelId: response.Model, innerContent: response);

    private static TextContent GetTextContentFromResponse(GenerateCompletionResponse response) => new(text: response.Response, modelId: response.Model, response, Encoding.UTF8);
}
