namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

/// <summary>
/// Hunyuan embedding generation service.
/// </summary>
public sealed class HunyuanTextEmbeddingGenerationService : ITextEmbeddingGenerationService
{
    private Dictionary<string, object?> AttributesInternal { get; } = [];

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    private readonly HunyuanClientCore _core;

    /// <summary>
    /// Initializes a new instance of the <see cref="HunyuanTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="model">The Hunyuan model for the chat completion service.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public HunyuanTextEmbeddingGenerationService(string model, string secretId, string secretKey, string? region = null, string? token = null, ILoggerFactory? loggerFactory = null)
        : this(model, secretId, secretKey, 60, region, token, loggerFactory)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HunyuanTextEmbeddingGenerationService"/> class.
    /// </summary>
    /// <param name="model">The Hunyuan model for the chat completion service.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="timeout">Time unit,default 60 seconds.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public HunyuanTextEmbeddingGenerationService(string model, string secretId, string secretKey, int timeout, string? region = null, string? token = null, ILoggerFactory? loggerFactory = null)
    {
        this._core = new HunyuanClientCore(
            model: model,
            credential: new Credential { SecretId = secretId, SecretKey = secretKey, Token = token },
            region: region ?? string.Empty,
            clientProfile: new ClientProfile
            {
                HttpProfile = new HttpProfile
                {
                    Timeout = timeout
                }
            },
            logger: loggerFactory?.CreateLogger(this.GetType()) ?? NullLogger.Instance);

        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    /// <inheritdoc/>
    public Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.GetEmbeddingsAsync(data, kernel, cancellationToken);
}