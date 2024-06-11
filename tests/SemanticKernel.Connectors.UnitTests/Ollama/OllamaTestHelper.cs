namespace SemanticKernel.Connectors.UnitTests.Ollama;

[ExcludeFromCodeCoverage]
internal static class OllamaTestHelper
{
    internal static class Constants
    {
        internal const string FakeUriString = "http://fake.uri";
        internal const string FakeModel = "fake-model";
        internal static readonly Uri FakeUri = new(FakeUriString);
        internal static readonly HttpClient FakeHttpClient = new() { BaseAddress = FakeUri };
        internal static readonly HttpClient FakeHttpClientWithNullBaseAddress = new() { BaseAddress = null };
    }

    internal static string GetTestResponse(string fileName)
    {
        return File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Ollama", "TestData", fileName));
    }
}
