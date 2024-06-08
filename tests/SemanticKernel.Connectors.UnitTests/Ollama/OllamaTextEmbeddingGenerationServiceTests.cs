namespace SemanticKernel.Connectors.UnitTests.Ollama;

public sealed class OllamaTextEmbeddingGenerationServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaTextEmbeddingGenerationServiceTests()
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
        OllamaTextEmbeddingGenerationService ollamaTextEmbeddingGenerationService = includeLoggerFactory
            ? new OllamaTextEmbeddingGenerationService("model", "http://localhost", loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaTextEmbeddingGenerationService("model", "http://localhost");

        Assert.NotNull(ollamaTextEmbeddingGenerationService);
        Assert.Equal("model", ollamaTextEmbeddingGenerationService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextEmbeddingGenerationService ollamaTextEmbeddingGenerationService = includeLoggerFactory
            ? new OllamaTextEmbeddingGenerationService("model", new Uri("http://localhost"), loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaTextEmbeddingGenerationService("model", new Uri("http://localhost"));

        Assert.NotNull(ollamaTextEmbeddingGenerationService);
        Assert.Equal("model", ollamaTextEmbeddingGenerationService.Attributes["ModelId"]);
    }

    public void Dispose()
    {
        this._httpClient.Dispose();
        this._messageHandlerStub.Dispose();
    }
}
