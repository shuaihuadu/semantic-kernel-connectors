namespace IdeaTech.SemanticKernel.Connectors.Ollama;

internal static partial class OllamaClientBuilder
{
    internal static OllamaClient CreateOllamaClient(HttpClient? httpClient, Uri? endpoint, ILoggerFactory? loggerFactory = null)
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

    internal static Uri GetOllamaClientEndpoint(HttpClient? httpClient, Uri? endpoint)
    {
        Verify.ValidateHttpClientAndEndpoint(httpClient, endpoint);

        if (!string.IsNullOrEmpty(httpClient?.BaseAddress?.AbsoluteUri))
        {
            return httpClient!.BaseAddress;
        }
        else
        {
            return endpoint!;
        }
    }
}