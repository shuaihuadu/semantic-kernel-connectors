using Microsoft.Extensions.Logging;
using Ollama.Core;

namespace Microsoft.SemanticKernel;

internal static partial class OllamaClientBuilder
{
    internal static OllamaClient CreateOllamaClient(HttpClient? httpClient, string? endpoint, ILoggerFactory? loggerFactory = null)
    {
        Verify.ValidateHttpClientAndEndpoint(httpClient, endpoint);

        if (!string.IsNullOrEmpty(httpClient?.BaseAddress?.AbsoluteUri))
        {
            return new OllamaClient(httpClient!, loggerFactory);
        }
        else
        {
            return new OllamaClient(endpoint!, loggerFactory);
        }
    }

    internal static string GetOllamaClientEndpointUrl(HttpClient? httpClient, string? endpoint)
    {
        Verify.ValidateHttpClientAndEndpoint(httpClient, endpoint);

        if (!string.IsNullOrEmpty(httpClient?.BaseAddress?.AbsoluteUri))
        {
            return httpClient!.BaseAddress.AbsoluteUri;
        }
        else
        {
            return endpoint!;
        }
    }

    internal static Uri GetOllamaClientEndpoint(HttpClient? httpClient, string? endpoint)
    {
        return new Uri(GetOllamaClientEndpointUrl(httpClient, endpoint));
    }
}