namespace Microsoft.SemanticKernel;

/// <summary>
/// Provides extension methods for the <see cref="IKernelBuilder"/> class to configure Ollama connectors.
/// </summary>
public static class OllamaKernelBuilderExtensions
{
    /// <summary>
    /// Adds a Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <param name="httpClient">The HttpClient to use with this service.</param>
    /// <returns></returns>
    public static IKernelBuilder AddOllamaTextGeneration(
        this IKernelBuilder builder,
        string model,
        Uri? endpoint = null,
        string? serviceId = null,
        HttpClient? httpClient = null)
    {
        Verify.NotNull(builder);
        Verify.NotNullOrWhiteSpace(model);

        builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId, (serviceProvider, _) => new OllamaTextGenerationService(
            model,
            endpoint,
            HttpClientProvider.GetHttpClient(httpClient, serviceProvider),
            serviceProvider.GetService<ILoggerFactory>()));

        return builder;
    }

    /// <summary>
    /// Adds a Ollama text generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="IKernelBuilder"/> instance to augment.</param>
    /// <param name="model">The name of the Ollama model.</param>
    /// <param name="endpoint">The endpoint URL for the text generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns></returns>
    public static IKernelBuilder AddOllamaTextGeneration(this IKernelBuilder builder, string model, string endpoint, string? serviceId = null)
    {
        return AddOllamaTextGeneration(builder, model, new Uri(endpoint), serviceId);
    }
}
