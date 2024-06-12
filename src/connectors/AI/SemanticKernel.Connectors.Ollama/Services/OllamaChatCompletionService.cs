namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama chat completion service.
/// </summary>
public sealed class OllamaChatCompletionService : IChatCompletionService
{
    private readonly OllamaClientCore _core;

    private Dictionary<string, object?> AttributesInternal { get; } = [];
    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaChatCompletionService(string model, Uri endpoint, ILoggerFactory? loggerFactory = null)
    {
        this._core = new OllamaClientCore(model, endpoint, loggerFactory);

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaChatCompletionService(string model, string endpoint, ILoggerFactory? loggerFactory = null) : this(model, new Uri(endpoint), loggerFactory) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="httpClient">HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaChatCompletionService(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
    {
        this._core = new OllamaClientCore(model, httpClient, loggerFactory);

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.CompleteChatMessageAsync(chatHistory, executionSettings, cancellationToken);

    /// <inheritdoc/>
    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.StreamCompleteChatMessageAsync(chatHistory, executionSettings, cancellationToken);
}
