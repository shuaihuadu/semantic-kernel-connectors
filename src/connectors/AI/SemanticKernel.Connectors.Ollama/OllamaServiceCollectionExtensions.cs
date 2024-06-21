// Copyright (c) IdeaTech. All rights reserved.

namespace Microsoft.SemanticKernel;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> class to configure Ollama connectors.
/// </summary>
public static class OllamaServiceCollectionExtensions
{
    #region Generation Completion

    /// <summary>
    /// Adds ann Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextGeneration(this IServiceCollection services, string model, Uri endpoint, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        services.AddKeyedSingleton<ITextGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextGenerationService(model, endpoint, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    /// <summary>
    /// Adds an Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextGeneration(this IServiceCollection services, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaTextGeneration(services, model, new Uri(endpoint), serviceId);
    }

    /// <summary>
    /// Adds ann Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextGeneration(this IServiceCollection services, string model, HttpClient httpClient, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        services.AddKeyedSingleton<ITextGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextGenerationService(model, httpClient, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    #endregion

    #region Chat Completion

    /// <summary>
    /// Adds an Ollama chat completion service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the chat completion service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaChatCompletion(this IServiceCollection services, string model, Uri endpoint, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) => new OllamaChatCompletionService(model, endpoint, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    /// <summary>
    /// Adds an Ollama chat completion service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the chat completion service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaChatCompletion(this IServiceCollection services, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaChatCompletion(services, model, new Uri(endpoint), serviceId);
    }

    /// <summary>
    /// Adds an Ollama chat completion service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaChatCompletion(this IServiceCollection services, string model, HttpClient httpClient, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        services.AddKeyedSingleton<IChatCompletionService>(serviceId, (serviceProvider, _) => new OllamaChatCompletionService(model, httpClient, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    #endregion

    #region Text Embedding

    /// <summary>
    /// Adds an Ollama text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama embedding model.</param>
    /// <param name="endpoint">The endpoint for the text embedding generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextEmbeddingGeneration(this IServiceCollection services, string model, Uri endpoint, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextEmbeddingGenerationService(model, endpoint, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    /// <summary>
    /// Adds an Ollama text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama embedding model.</param>
    /// <param name="endpoint">The endpoint for the text embedding generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextEmbeddingGeneration(this IServiceCollection services, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaTextEmbeddingGeneration(services, model, new Uri(endpoint), serviceId);
    }

    /// <summary>
    /// Adds an Ollama text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama embedding model.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaTextEmbeddingGeneration(this IServiceCollection services, string model, HttpClient httpClient, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        services.AddKeyedSingleton<ITextEmbeddingGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextEmbeddingGenerationService(model, httpClient, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    #endregion

    #region Image To Text

    /// <summary>
    /// Adds ann Ollama image-to-text service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaImageToText(this IServiceCollection services, string model, Uri endpoint, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        services.AddKeyedSingleton<IImageToTextService>(serviceId, (serviceProvider, _) => new OllamaImageToTextService(model, endpoint, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    /// <summary>
    /// Adds ann Ollama image-to-text service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaImageToText(this IServiceCollection services, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaImageToText(services, model, new Uri(endpoint), serviceId);
    }

    /// <summary>
    /// Adds ann Ollama image-to-text service with the specified configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The same instance as <paramref name="services"/>.</returns>
    public static IServiceCollection AddOllamaImageToText(this IServiceCollection services, string model, HttpClient httpClient, string? serviceId = null)
    {
        Verify.NotNull(services);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        services.AddKeyedSingleton<IImageToTextService>(serviceId, (serviceProvider, _) => new OllamaImageToTextService(model, httpClient, serviceProvider.GetService<ILoggerFactory>()));

        return services;
    }

    #endregion
}
