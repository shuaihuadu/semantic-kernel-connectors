namespace SemanticKernel.Connectors.Ollama.UnitTests.Services;

public sealed class OllamaImageToTextServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly ImageContent _imageContent;

    public OllamaImageToTextServiceTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(OllamaTestHelper.GetTestResponse("image_to_text_test_response.json"));

        this._httpClient = new HttpClient(this._messageHandlerStub, false)
        {
            BaseAddress = TestConstants.FakeUri
        };
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
        this._imageContent = new ImageContent
        {
            Data = new ReadOnlyMemory<byte>([1, 2, 3])
        };
    }

    #region Constructors

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriStringWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaImageToTextService ollamaImageToTextService = includeLoggerFactory
            ? new OllamaImageToTextService(TestConstants.FakeModel, TestConstants.FakeUriString, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaImageToTextService(TestConstants.FakeModel, TestConstants.FakeUriString);

        Assert.NotNull(ollamaImageToTextService);
        Assert.Equal(TestConstants.FakeModel, ollamaImageToTextService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaImageToTextService ollamaImageToTextService = includeLoggerFactory
            ? new OllamaImageToTextService(TestConstants.FakeModel, TestConstants.FakeUri, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaImageToTextService(TestConstants.FakeModel, TestConstants.FakeUri);

        Assert.NotNull(ollamaImageToTextService);
        Assert.Equal(TestConstants.FakeModel, ollamaImageToTextService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithHttpClientWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaImageToTextService ollamaImageToTextService = includeLoggerFactory
            ? new OllamaImageToTextService(TestConstants.FakeModel, TestConstants.FakeHttpClient, loggerFactory: this._mockLoggerFactory.Object)
            : new OllamaImageToTextService(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Assert.NotNull(ollamaImageToTextService);
        Assert.Equal(TestConstants.FakeModel, ollamaImageToTextService.Attributes["ModelId"]);
    }

    #endregion

    [Fact]
    public async Task GetTextContentsWorksCorrectlyAsync()
    {
        OllamaImageToTextService ollamaImageToTextService = new(TestConstants.FakeModel, _httpClient);

        IReadOnlyList<TextContent> textContents = await ollamaImageToTextService.GetTextContentsAsync(this._imageContent);

        Assert.NotNull(textContents);
        Assert.True(textContents.Count > 0);
        Assert.Equal("This is a test image to text response", textContents[0].Text);
    }

    [Fact]
    public async Task GetTextContentsHandlesSettingCorrectlyAsync()
    {
        OllamaImageToTextService ollamaImageToTextService = new(TestConstants.FakeModel, _httpClient);

        OllamaPromptExecutionSettings executionSettings = new()
        {
            MaxTokens = 100,
            Temperature = 0.5,
            TopP = 0.2,
            TopK = 100,
            FrequencyPenalty = 1.2,
            PresencePenalty = 1.4,
            Seed = 110,
            KeepAlive = 500,
            SystemPrompt = "You are an AI Assistant",
            Stop = ["stop_sequence"],
            Format = "json",
            ModelId = null
        };

        IReadOnlyList<TextContent> textContents = await ollamaImageToTextService.GetTextContentsAsync(this._imageContent, executionSettings);

        Assert.NotNull(textContents);
        Assert.True(textContents.Count > 0);

        byte[]? requestContent = this._messageHandlerStub.RequestContent;

        Assert.NotNull(requestContent);

        JsonElement content = JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(requestContent));

        Assert.Equal(TestConstants.FakeModel, content.GetProperty("model").GetString());
        Assert.Equal("What is in this image?", content.GetProperty("prompt").GetString());
        Assert.Equal("You are an AI Assistant", content.GetProperty("system").GetString());
        Assert.Equal(500, content.GetProperty("keep_alive").GetInt32());
        Assert.Equal("json", content.GetProperty("format").GetString());

        Assert.Equal(100, content.GetProperty("options").GetProperty("num_ctx").GetInt32());
        Assert.Equal(0.5, content.GetProperty("options").GetProperty("temperature").GetDouble());
        Assert.Equal(0.2, content.GetProperty("options").GetProperty("top_p").GetDouble());
        Assert.Equal(100, content.GetProperty("options").GetProperty("top_k").GetInt32());
        Assert.Equal(1.4, content.GetProperty("options").GetProperty("presence_penalty").GetDouble());
        Assert.Equal(1.2, content.GetProperty("options").GetProperty("frequency_penalty").GetDouble());
        Assert.Equal(110, content.GetProperty("options").GetProperty("seed").GetInt32());
        Assert.Equal("stop_sequence", content.GetProperty("options").GetProperty("stop")[0].GetString());
    }

    [Fact]
    public async Task ShouldHandleMetadataAsync()
    {
        OllamaImageToTextService ollamaImageToTextService = new(TestConstants.FakeModel, _httpClient);

        IReadOnlyList<TextContent> textContents = await ollamaImageToTextService.GetTextContentsAsync(this._imageContent);

        Assert.NotNull(textContents);
        Assert.True(textContents.Count > 0);

        TextContent content = textContents.SingleOrDefault()!;

        Assert.NotNull(content);
        Assert.Equal("llama3", content.ModelId);
        Assert.IsType<OllamaTextGenerationMetadata>(content.Metadata);

        OllamaTextGenerationMetadata? metadata = content.Metadata as OllamaTextGenerationMetadata;

        Assert.Equal("This is a test image to text response", content.Text);
        Assert.Null(metadata!.Context);
        Assert.Equal(4233976012, metadata.TotalDuration);
        Assert.Equal(819378, metadata.LoadDuration);
        Assert.Equal(0, metadata.PromptEvalCount);
        Assert.Equal(100559000, metadata.PromptEvalDuration);
        Assert.Equal(26, metadata.EvalCount);
        Assert.Equal(3042076000, metadata.EvalDuration);
        Assert.Equal("stop", metadata.DoneReason);

        Assert.True(metadata.Done);

        _ = DateTimeOffset.TryParse("2024-06-10T12:31:55.6058572+00:00", out DateTimeOffset date);
        Assert.True(metadata.CreatedAt == date);
    }

    [Fact]
    public async Task GetTextContentsAsyncShouldThrowWithHttpStatusIsNotOK()
    {
        OllamaImageToTextService ollamaTextGenerationService = new(TestConstants.FakeModel, _httpClient);

        this._messageHandlerStub.ResponseToReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);

        await Assert.ThrowsAsync<OllamaHttpOperationException>(() => ollamaTextGenerationService.GetTextContentsAsync(this._imageContent));
    }

    public void Dispose()
    {
        this._messageHandlerStub.Dispose();
        this._httpClient.Dispose();
    }
}