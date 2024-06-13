namespace SemanticKernel.Connectors.Ollama.UnitTests.OllamaSdk;

public class OllamaClientCoreTests
{
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaClientCoreTests()
    {
        this._httpClient = new HttpClient()
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
        OllamaClientCore core = includeLoggerFactory
            ? new OllamaClientCore(TestConstants.FakeModel, TestConstants.FakeUriString, loggerFactory: _mockLoggerFactory.Object)
            : new OllamaClientCore(TestConstants.FakeModel, TestConstants.FakeUriString);

        Assert.NotNull(core);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaClientCore core = includeLoggerFactory
            ? new OllamaClientCore(TestConstants.FakeModel, TestConstants.FakeUri, loggerFactory: _mockLoggerFactory.Object)
            : new OllamaClientCore(TestConstants.FakeModel, TestConstants.FakeUri);

        Assert.NotNull(core);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithHttpClientWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaClientCore core = includeLoggerFactory
            ? new OllamaClientCore(TestConstants.FakeModel, TestConstants.FakeHttpClient, loggerFactory: _mockLoggerFactory.Object)
            : new OllamaClientCore(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Assert.NotNull(core);
    }
}