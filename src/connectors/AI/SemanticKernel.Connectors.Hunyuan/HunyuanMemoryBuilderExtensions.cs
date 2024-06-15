namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

/// <summary>
/// Provides extension methods for the <see cref="MemoryBuilder"/> class to configure Hunyuan connectors.
/// </summary>
public static class HunyuanMemoryBuilderExtensions
{
    /// <summary>
    /// Adds ann Hunyuan text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="MemoryBuilder"/> instance.</param>
    /// <param name="model">The name of the Hunyuan model.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="timeout">Time unit,default 60 seconds.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static MemoryBuilder WithHunyuanTextEmbeddingGeneration(this MemoryBuilder builder, string model, string secretId, string secretKey, int timeout = 60, string? region = null, string? token = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNullOrWhiteSpace(secretId);
        Verify.NotNullOrWhiteSpace(secretKey);

        return builder.WithTextEmbeddingGeneration((loggerFactory, _) => new HunyuanTextEmbeddingGenerationService(model, secretId, secretKey, timeout, region, token, loggerFactory));
    }
}