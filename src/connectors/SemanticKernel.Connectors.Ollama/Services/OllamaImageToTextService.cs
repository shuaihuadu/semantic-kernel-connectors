namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama image to text service
/// </summary>
public class OllamaImageToTextService : OllamaTextBaseService, IImageToTextService
{
    private Dictionary<string, object?> AttributesInternal { get; } = [];

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaImageToTextService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaImageToTextService(string model, Uri endpoint, ILoggerFactory? loggerFactory = null) : base(model, endpoint, loggerFactory)
    {
        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaImageToTextService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaImageToTextService(string model, string endpoint, ILoggerFactory? loggerFactory = null) : this(model, new Uri(endpoint), loggerFactory) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaImageToTextService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="httpClient">HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaImageToTextService(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null) : base(model, httpClient, loggerFactory)
    {
        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Due to the <see cref="IImageToTextService"/> current not support for adding prompt to images, So temporarily use the default Prompt："What is in this image?"
    /// </remarks>
    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(ImageContent content, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        Verify.NotNull(content, nameof(content));
        Verify.NotNull(content.Data, nameof(content.Data));
        Verify.NotNullOrEmpty(content.Data.Value.ToArray(), nameof(content.Data));

        const string prompt = "What is in this image?";

        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);
        ollamaPromptExecutionSettings.ModelId ??= this._model;

        string imageBase64Data = Convert.ToBase64String(content.Data.Value.ToArray());

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, prompt, executionSettings);

        GenerateCompletionResponse response;

        try
        {
            using OllamaClient client = new(this._httpClient, this._loggerFactory);

            response = await client.GenerateCompletionAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings, [imageBase64Data]), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        TextContent textContent = GetTextContentFromResponse(response);

        activity?.SetCompletionResponse([textContent]);

        return [textContent];
    }
}