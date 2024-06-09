namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama embedding generation service.
/// </summary>
public sealed class OllamaTextEmbeddingGenerationService : ITextEmbeddingGenerationService
{
    private Dictionary<string, object?> AttributesInternal { get; } = [];

    private readonly Uri? _endpoint;
    private readonly HttpClient _httpClient;
    private readonly string _model;
    private readonly ILoggerFactory? _loggerFactory;

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextEmbeddingGenerationService(string model, Uri endpoint, ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model, nameof(model));
        Verify.NotNull(endpoint, nameof(endpoint));

        this._model = model;
        this._loggerFactory = loggerFactory;
        this._endpoint = endpoint;
        this._httpClient = new()
        {
            BaseAddress = this._endpoint,
        };

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextEmbeddingGenerationService(string model, string endpoint, ILoggerFactory? loggerFactory = null) : this(model, new Uri(endpoint), loggerFactory)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="httpClient">Optional HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public OllamaTextEmbeddingGenerationService(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model, nameof(model));
        Verify.NotNull(httpClient, nameof(httpClient));
        Verify.NotNull(httpClient.BaseAddress, nameof(httpClient.BaseAddress));
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress.AbsoluteUri, nameof(httpClient.BaseAddress.AbsoluteUri));

        this._model = model;
        this._httpClient = httpClient;
        this._loggerFactory = loggerFactory;
        this._endpoint = this._httpClient.BaseAddress;

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <inheritdoc/>
    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        Verify.NotNullOrEmpty(data, nameof(data));

        if (data.Count > 1)
        {
            throw new NotSupportedException("Currently this interface does not support multiple embeddings results per data item, use only one data item");
        }

        using OllamaClient client = new(this._httpClient, this._loggerFactory);

        EmbeddingResponse response = await client.GenerateEmbeddingAsync(this._model, data.First(), cancellationToken);

        return [response.Embedding];
    }
}
