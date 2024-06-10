namespace SemanticKernel.Connectors.UnitTests.Ollama.Services;

public sealed class OllamaTextEmbeddingGenerationServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaTextEmbeddingGenerationServiceTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(OllamaTestHelper.GetTestResponse("text_embedding_test_response.json"));
        this._httpClient = new HttpClient(_messageHandlerStub, false)
        {
            BaseAddress = TestConstants.FakeUri
        };
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriStringWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextEmbeddingGenerationService ollamaTextEmbeddingGenerationService = includeLoggerFactory
            ? new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeUriString, loggerFactory: this._mockLoggerFactory.Object)
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
            ? new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeUri, loggerFactory: this._mockLoggerFactory.Object)
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
    ? new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeHttpClient, loggerFactory: this._mockLoggerFactory.Object)
    : new OllamaTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    [Fact]
    public async Task ShouldHandleServiceResponseAsync()
    {
        OllamaTextEmbeddingGenerationService ollamaTextGenerationService = new(TestConstants.FakeModel, this._httpClient);

        IList<ReadOnlyMemory<float>> embeddings = await ollamaTextGenerationService.GenerateEmbeddingsAsync(["hello"]);

        Assert.NotNull(embeddings);
        Assert.Single(embeddings);
        Assert.Equal(384, embeddings.First().Length);
    }

    [Fact]
    public async Task ShouldThrowWithInvalidDataAsync()
    {
        OllamaTextEmbeddingGenerationService ollamaTextGenerationService = new(TestConstants.FakeModel, this._httpClient);

        await Assert.ThrowsAsync<ArgumentException>(() => ollamaTextGenerationService.GenerateEmbeddingsAsync([]));

        await Assert.ThrowsAsync<NotSupportedException>(() => ollamaTextGenerationService.GenerateEmbeddingsAsync(["hello", "world"]));
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _messageHandlerStub.Dispose();
    }
}
