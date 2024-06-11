namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama base service.
/// </summary>
public abstract class OllamaBaseService
{
    /// <summary>
    /// The model provider.
    /// </summary>
    protected const string ModelProvider = "ollama";

    /// <summary>
    /// The uri endpoint including the port where Ollama server is hosted
    /// </summary>
    protected readonly Uri? _endpoint;
    /// <summary>
    /// The model request http client.
    /// </summary>
    protected readonly HttpClient _httpClient;
    /// <summary>
    /// The model name.
    /// </summary>
    protected readonly string _model;
    /// <summary>
    /// The logger factory to be used for logging.
    /// </summary>
    protected readonly ILoggerFactory? _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaBaseService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    protected OllamaBaseService(string model, Uri endpoint, ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model, nameof(model));
        Verify.NotNull(endpoint, nameof(endpoint));
        Verify.NotNullOrWhiteSpace(endpoint.AbsoluteUri, nameof(endpoint.AbsoluteUri));

        this._model = model;
        this._loggerFactory = loggerFactory;
        this._endpoint = endpoint;
        this._httpClient = new()
        {
            BaseAddress = this._endpoint,
        };
    }

    /*
    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextBaseService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    protected OllamaBaseService(string model, string endpoint, ILoggerFactory? loggerFactory = null) : this(model, new Uri(endpoint), loggerFactory) { }
    */

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextBaseService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="httpClient">HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    protected OllamaBaseService(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model, nameof(model));
        Verify.NotNull(httpClient, nameof(httpClient));
        Verify.NotNull(httpClient.BaseAddress, nameof(httpClient.BaseAddress));
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress.AbsoluteUri, nameof(httpClient.BaseAddress.AbsoluteUri));

        this._model = model;
        this._httpClient = httpClient;
        this._loggerFactory = loggerFactory;
        this._endpoint = this._httpClient.BaseAddress;
    }
}
