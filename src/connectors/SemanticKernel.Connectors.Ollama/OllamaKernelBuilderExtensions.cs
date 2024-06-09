namespace Microsoft.SemanticKernel;

/// <summary>
/// Provides extension methods for the <see cref="IKernelBuilder"/> class to configure Ollama connectors.
/// </summary>
public static class OllamaKernelBuilderExtensions
{
    #region Generation Completion

    /// <summary>
    /// Adds ann Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextGeneration(this IKernelBuilder builder, string model, Uri endpoint, string? serviceId = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextGenerationService(model, endpoint, serviceProvider.GetService<ILoggerFactory>()));

        return builder;
    }

    /// <summary>
    /// Adds an Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextGeneration(this IKernelBuilder builder, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaTextGeneration(builder, model, new Uri(endpoint), serviceId);
    }

    /// <summary>
    /// Adds ann Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextGeneration(this IKernelBuilder builder, string model, HttpClient httpClient, string? serviceId = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextGenerationService(model, httpClient, serviceProvider.GetService<ILoggerFactory>()));

        return builder;
    }

    #endregion

    #region Chat Completion

    /// <summary>
    /// Adds an Ollama chat completion service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the chat completion service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaChatCompletion(this IKernelBuilder builder, string model, Uri endpoint, string? serviceId = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) => new OllamaChatCompletionService(model, endpoint, serviceProvider.GetService<ILoggerFactory>()));

        return builder;
    }


    /// <summary>
    /// Adds an Ollama chat completion service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the chat completion service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaChatCompletion(this IKernelBuilder builder, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaChatCompletion(builder, model, new Uri(endpoint), serviceId);
    }

    /// <summary>
    /// Adds an Ollama chat completion service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaChatCompletion(this IKernelBuilder builder, string model, HttpClient httpClient, string? serviceId = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) => new OllamaChatCompletionService(model, httpClient, serviceProvider.GetService<ILoggerFactory>()));

        return builder;
    }
    #endregion

    #region Text Embedding

    /// <summary>
    /// Adds an Ollama text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama embedding model.</param>
    /// <param name="endpoint">The endpoint for the text embedding generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextEmbeddingGeneration(this IKernelBuilder builder, string model, Uri endpoint, string? serviceId = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        builder.Services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextEmbeddingGenerationService(model, endpoint, serviceProvider.GetService<ILoggerFactory>()));

        return builder;
    }

    /// <summary>
    /// Adds an Ollama text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama embedding model.</param>
    /// <param name="endpoint">The endpoint for the text embedding generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextEmbeddingGeneration(this IKernelBuilder builder, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaTextEmbeddingGeneration(builder, model, new Uri(endpoint), serviceId);
    }


    /// <summary>
    /// Adds an Ollama text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama embedding model.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static IKernelBuilder AddOllamaTextEmbeddingGeneration(this IKernelBuilder builder, string model, HttpClient httpClient, string? serviceId = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        builder.Services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextEmbeddingGenerationService(model, httpClient, serviceProvider.GetService<ILoggerFactory>()));

        return builder;
    }

    #endregion
}
