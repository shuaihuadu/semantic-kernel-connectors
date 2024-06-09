namespace SemanticKernel.Connectors.UnitTests;

[ExcludeFromCodeCoverage]
internal static class OllamaTestHelper
{
    internal static class Constants
    {
        internal const string FakeUriString = "http://fake.uri";
        internal const string FakeModel = "model";
        internal static readonly Uri FakeUri = new(FakeUriString);
        internal static readonly HttpClient FakeHttpClient = new() { BaseAddress = FakeUri };
    }

    internal static string GetTestResponse(string fileName)
    {
        return File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Ollama", "TestData", fileName));
    }
}
