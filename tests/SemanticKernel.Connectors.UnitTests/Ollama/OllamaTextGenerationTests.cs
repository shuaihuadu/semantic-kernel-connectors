namespace SemanticKernel.Connectors.UnitTests.Ollama;

public sealed class OllamaTextGenerationTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaTextGenerationTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._httpClient = new HttpClient(this._messageHandlerStub, false);
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextGenerationService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaTextGenerationService("model", "http://localhost", loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaTextGenerationService("model", "http://localhost");

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal("model", ollamaTextGenerationService.Attributes["ModelId"]);
    }

    public void Dispose()
    {
        this._messageHandlerStub.Dispose();
        this._httpClient.Dispose();
    }
}