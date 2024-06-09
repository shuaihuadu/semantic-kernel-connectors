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
            ? new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeUriString, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeUriString);

        Assert.NotNull(ollamaChatCompletionService);
        Assert.Equal(TestConstants.FakeModel, ollamaChatCompletionService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaChatCompletionService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeUri, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeUri);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithHttpClientWorksCorrectly(bool includeLoggerFactory)
    {
        this._httpClient.BaseAddress = TestConstants.FakeUri;

        OllamaChatCompletionService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaChatCompletionService(TestConstants.FakeModel, this._httpClient, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaChatCompletionService(TestConstants.FakeModel, this._httpClient);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    public void Dispose()
    {
        this._httpClient.Dispose();
        this._messageHandlerStub.Dispose();
    }
}
