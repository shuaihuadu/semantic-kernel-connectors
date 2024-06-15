namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

/// <summary>
/// Hunyuan chat completion service.
/// </summary>
public class HunyuanChatCompletionService : IChatCompletionService, ITextGenerationService
{
    private Dictionary<string, object?> AttributesInternal { get; } = [];

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?> Attributes => this.AttributesInternal;

    private readonly HunyuanClientCore _core;

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
    public HunyuanChatCompletionService(string model, string secretId, string secretKey, int timeout = 60, string? region = null, string? token = null, ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNullOrWhiteSpace(secretId);
        Verify.NotNullOrWhiteSpace(secretKey);
        Verify.GreatThan(timeout, 0);

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

    /// <inheritdoc />
    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.GetChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.GetStreamingChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken);

    /// <inheritdoc />
    public Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.GetChatAsTextContentsAsync(prompt, executionSettings, kernel, cancellationToken);

    /// <inheritdoc />
    public IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
        => this._core.GetChatAsTextStreamingContentsAsync(prompt, executionSettings, kernel, cancellationToken);
}