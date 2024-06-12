namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama image to text service
/// </summary>
public class OllamaImageToTextService : IImageToTextService
{
    private readonly OllamaClientCore _core;

    private Dictionary<string, object?> AttributesInternal { get; } = [];

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaImageToTextService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaImageToTextService(string model, Uri endpoint, ILoggerFactory? loggerFactory = null)
    {
        this._core = new OllamaClientCore(model, endpoint, loggerFactory);

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
    public OllamaImageToTextService(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
    {
        this._core = new OllamaClientCore(model, httpClient, loggerFactory);

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Due to the <see cref="IImageToTextService"/> current not support for adding prompt to images, So temporarily use the default Prompt："What is in this image?"
    /// </remarks>
    public Task<IReadOnlyList<TextContent>> GetTextContentsAsync(ImageContent content, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.GenerateTextFromImageAsync(content, executionSettings, cancellationToken);
}