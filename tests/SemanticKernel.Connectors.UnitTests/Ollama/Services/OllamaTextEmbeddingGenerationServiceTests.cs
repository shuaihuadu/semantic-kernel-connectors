namespace SemanticKernel.Connectors.UnitTests.Ollama.Services;

public sealed class OllamaTextEmbeddingGenerationServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaTextEmbeddingGenerationServiceTests()
    {
        _messageHandlerStub = new HttpMessageHandlerStub();
        _httpClient = new HttpClient(_messageHandlerStub, false);
        _mockLoggerFactory = new Mock<ILoggerFactory>();
    }
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriStringWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextEmbeddingGenerationService ollamaTextEmbeddingGenerationService = includeLoggerFactory
            ? new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeUriString, loggerFactory: _mockLoggerFactory.Object)
            : new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeUriString);

        Assert.NotNull(ollamaTextEmbeddingGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextEmbeddingGenerationService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextEmbeddingGenerationService ollamaTextEmbeddingGenerationService = includeLoggerFactory
            ? new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeUri, loggerFactory: _mockLoggerFactory.Object)
            : new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeUri);

        Assert.NotNull(ollamaTextEmbeddingGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextEmbeddingGenerationService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithHttpClientWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextEmbeddingGenerationService ollamaTextGenerationService = includeLoggerFactory
    ? new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeHttpClient, loggerFactory: _mockLoggerFactory.Object)
    : new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _messageHandlerStub.Dispose();
    }
}
