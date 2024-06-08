namespace SemanticKernel.Connectors.UnitTests.Ollama;

public sealed class OllamaChatCompletionServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaChatCompletionServiceTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._httpClient = new HttpClient(this._messageHandlerStub, false);
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriStringWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaChatCompletionService ollamaChatCompletionService = includeLoggerFactory
            ? new OllamaChatCompletionService("model", "http://localhost", loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaChatCompletionService("model", "http://localhost");

        Assert.NotNull(ollamaChatCompletionService);
        Assert.Equal("model", ollamaChatCompletionService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaChatCompletionService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaChatCompletionService("model", new Uri("http://localhost"), loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaChatCompletionService("model", new Uri("http://localhost"));

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal("model", ollamaTextGenerationService.Attributes["ModelId"]);
    }

    public void Dispose()
    {
        this._httpClient.Dispose();
        this._messageHandlerStub.Dispose();
    }
}
