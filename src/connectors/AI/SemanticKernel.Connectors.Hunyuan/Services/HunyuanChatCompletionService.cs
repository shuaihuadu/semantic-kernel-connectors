namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

/// <summary>
/// Hunyuan chat completion service.
/// </summary>
public class HunyuanChatCompletionService : IChatCompletionService
{

    private Dictionary<string, object?> AttributesInternal { get; } = [];

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    private HunyuanClientCore Client { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HunyuanChatCompletionService"/> class.
    /// </summary>
    /// <param name="model">The Hunyuan model for the chat completion service.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public HunyuanChatCompletionService(string model, string secretId, string secretKey, string? region = null, string? token = null, ILoggerFactory? loggerFactory = null)
        : this(model, secretId, secretKey, 60, region, token, loggerFactory)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HunyuanChatCompletionService"/> class.
    /// </summary>
    /// <param name="model">The Hunyuan model for the chat completion service.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="timeout">Time unit,default 60 seconds.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public HunyuanChatCompletionService(string model, string secretId, string secretKey, int timeout, string? region = null, string? token = null, ILoggerFactory? loggerFactory = null)
    {
        this.Client = new HunyuanClientCore(
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

    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default) => this.Client.CompleteChatMessageAsync(chatHistory, executionSettings);

    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default) => this.Client.StreamingCompleteChatMessageAsync(chatHistory, executionSettings);
}