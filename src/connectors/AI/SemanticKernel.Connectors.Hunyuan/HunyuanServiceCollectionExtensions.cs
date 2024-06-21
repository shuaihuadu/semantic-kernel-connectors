// Copyright (c) IdeaTech. All rights reserved.

namespace Microsoft.SemanticKernel;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> class to configure Hunyuan connectors.
/// </summary>
public static class HunyuanServiceCollectionExtensions
{
    #region Generation Completion

    /// <summary>
    /// Adds an Hunyuan text generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Hunyuan model.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="timeout">Time unit,default 60 seconds.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddHunyuanTextGeneration(this IServiceCollection services, string model, string secretId, string secretKey, int timeout = 60, string? region = null, string? token = null, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNullOrWhiteSpace(secretId);
        Verify.NotNullOrWhiteSpace(secretKey);

        services.AddKeyedSingleton<ITextGenerationService>(serviceId, (serviceProvider, _) => new HunyuanChatCompletionService(model, secretId, secretKey, timeout, region, token, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    #endregion

    #region Chat Completion

    /// <summary>
    /// Adds an Hunyuan chat completion service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Hunyuan model.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="timeout">Time unit,default 60 seconds.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddHunyuanChatCompletion(this IServiceCollection services, string model, string secretId, string secretKey, int timeout = 60, string? region = null, string? token = null, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNullOrWhiteSpace(secretId);
        Verify.NotNullOrWhiteSpace(secretKey);
        Verify.GreatThan(timeout, 0);

        services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) => new HunyuanChatCompletionService(model, secretId, secretKey, timeout, region, token, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    #endregion

    #region Text Embedding

    /// <summary>
    /// Adds an Hunyuan text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Hunyuan model.</param>
    /// <param name="secretId">SecretId, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="secretKey">SecretKey, can only be obtained from Tencent Cloud Management Console.</param>
    /// <param name="timeout">Time unit,default 60 seconds.</param>
    /// <param name="region">Region name, such as "ap-guangzhou".</param>
    /// <param name="token">Optional</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddHunyuanTextEmbeddingGeneration(this IServiceCollection services, string model, string secretId, string secretKey, int timeout = 60, string? region = null, string? token = null, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNullOrWhiteSpace(secretId);
        Verify.NotNullOrWhiteSpace(secretKey);

        services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId, (serviceProvider, _) => new HunyuanTextEmbeddingGenerationService(model, secretId, secretKey, timeout, region, token, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    #endregion
}
