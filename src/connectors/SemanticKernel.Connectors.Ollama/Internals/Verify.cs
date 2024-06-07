namespace Microsoft.SemanticKernel;

internal static partial class Verify
{
    internal static void ValidateHttpClientAndEndpoint(HttpClient? httpClient, Uri? endpoint)
    {
        string message = $"The {nameof(httpClient)}.{nameof(HttpClient.BaseAddress)} and {nameof(endpoint)} are both null or empty. Please ensure at least one is provided.";

        if (string.IsNullOrEmpty(httpClient?.BaseAddress?.AbsoluteUri) && endpoint is null && string.IsNullOrEmpty(endpoint?.AbsoluteUri))
        {
            throw new ArgumentException(message);
        }
    }
}