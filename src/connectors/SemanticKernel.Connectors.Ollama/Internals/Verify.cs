namespace Microsoft.SemanticKernel;

internal static partial class Verify
{
    internal static void ValidateHttpClientAndEndpoint(HttpClient? httpClient, string? endpoint)
    {
        string message = $"The {nameof(httpClient)}.{nameof(HttpClient.BaseAddress)} and {nameof(endpoint)} are both null or empty. Please ensure at least one is provided.";

        if (string.IsNullOrEmpty(httpClient?.BaseAddress?.AbsoluteUri) && string.IsNullOrEmpty(endpoint))
        {
            throw new ArgumentException(message);
        }

        if (string.IsNullOrEmpty(httpClient?.BaseAddress?.AbsoluteUri))
        {
            ValidateUrl(endpoint!, paramName: nameof(endpoint));
        }
    }
}