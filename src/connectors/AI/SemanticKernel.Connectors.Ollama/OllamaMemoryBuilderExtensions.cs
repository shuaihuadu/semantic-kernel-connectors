// Copyright (c) IdeaTech. All rights reserved.

namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Provides extension methods for the <see cref="MemoryBuilder"/> class to configure Ollama connectors.
/// </summary>
public static class OllamaMemoryBuilderExtensions
{
    /// <summary>
    /// Adds ann Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="MemoryBuilder"/> instance.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static MemoryBuilder WithOllamaTextEmbeddingGeneration(this MemoryBuilder builder, string model, Uri endpoint)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsolutePath);

        return builder.WithTextEmbeddingGeneration((loggerFactory, _) => new OllamaTextEmbeddingGenerationService(model, endpoint, loggerFactory));
    }

    /// <summary>
    /// Adds an Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="MemoryBuilder"/> instance.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static MemoryBuilder WithOllamaTextEmbeddingGeneration(this MemoryBuilder builder, string model, string endpoint)
    {
        return WithOllamaTextEmbeddingGeneration(builder, model, new Uri(endpoint));
    }

    /// <summary>
    /// Adds ann Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="MemoryBuilder"/> instance.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <returns>The same instance as <paramref name="builder"/>.</returns>
    public static MemoryBuilder WithOllamaTextEmbeddingGeneration(this MemoryBuilder builder, string model, HttpClient httpClient)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress?.AbsolutePath);

        return builder.WithTextEmbeddingGeneration((loggerFactory, _) => new OllamaTextEmbeddingGenerationService(model, httpClient, loggerFactory));
    }
}
