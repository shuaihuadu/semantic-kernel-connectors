namespace SemanticKernel.Connectors.UnitTests.Ollama;

public sealed class OllamaTextGenerationTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaTextGenerationTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(OllamaTestHelper.GetTestResponse("textgeneration_test_response.json"));

        this._httpClient = new HttpClient(this._messageHandlerStub, false)
        {
            BaseAddress = TestConstants.FakeUri
        };
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }

    #region Constructors

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriStringWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextGenerationService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaTextGenerationService(TestConstants.FakeModel, TestConstants.FakeUriString, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaTextGenerationService(TestConstants.FakeModel, TestConstants.FakeUriString);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextGenerationService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaTextGenerationService(TestConstants.FakeModel, TestConstants.FakeUri, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaTextGenerationService(TestConstants.FakeModel, TestConstants.FakeUri);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal("model", ollamaTextGenerationService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithHttpClientWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaTextGenerationService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaTextGenerationService(TestConstants.FakeModel, TestConstants.FakeHttpClient, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaTextGenerationService(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    #endregion

    [Fact]
    public async Task GetTextContentsWorksCorrectlyAsync()
    {
        OllamaTextGenerationService ollamaTextGenerationService = new(TestConstants.FakeModel, this._httpClient);

        IReadOnlyList<TextContent> textContents = await ollamaTextGenerationService.GetTextContentsAsync("Prompt");

        Assert.True(textContents.Count > 0);
        Assert.Equal("This is a test generation response", textContents[0].Text);
    }

    [Fact]
    public async Task ShouldHandleMetadataAsync()
    {
        OllamaTextGenerationService ollamaTextGenerationService = new(TestConstants.FakeModel, this._httpClient);

        IReadOnlyList<TextContent> textContents = await ollamaTextGenerationService.GetTextContentsAsync("Prompt");

        Assert.NotNull(textContents);
        Assert.NotEmpty(textContents);

        TextContent content = textContents.SingleOrDefault()!;

        Assert.NotNull(content);
        Assert.IsType<OllamaTextGenerationMetadata>(content.Metadata);

        OllamaTextGenerationMetadata? meta = content.Metadata as OllamaTextGenerationMetadata;

        Assert.Equal("This is a test generation response", content.Text);
        Assert.True(meta!.Context!.Length > 0);
        Assert.Equal(4285976012, meta.TotalDuration);
        Assert.Equal(819378, meta.LoadDuration);
        Assert.Equal(10, meta.PromptEvalCount);
        Assert.Equal(200559000, meta.PromptEvalDuration);
        Assert.Equal(26, meta.EvalCount);
        Assert.Equal(4042076000, meta.EvalDuration);
        Assert.Equal("stop", meta.DoneReason);

        Assert.True(meta.Done);

        DateTimeOffset.TryParse("2024-06-09T02:24:37.6058572+00:00", out DateTimeOffset date);
        Assert.True(meta.CreatedAt == date);
    }

    public void Dispose()
    {
        this._messageHandlerStub.Dispose();
        this._httpClient.Dispose();
    }
}