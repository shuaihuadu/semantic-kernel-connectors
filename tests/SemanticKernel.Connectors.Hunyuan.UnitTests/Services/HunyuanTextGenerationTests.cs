namespace SemanticKernel.Connectors.Hunyuan.UnitTests.Services;

public sealed class HunyuanTextGenerationTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public HunyuanTextGenerationTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(HunyuanTestHelper.GetTestResponse("chat_completion_test_response.json"));
        this._httpClient = new HttpClient(this._messageHandlerStub, false);
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }


    [Fact]
    public async Task GetChatMessageContentHandlesSettingCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        HunyuanPromptExecutionSettings executionSettings = new()
        {
            EnableEnhancement = true,
            Temperature = 0.9f,
            StreamModeration = true,
            Stream = false,
            TopP = 0.3f
        };

        IReadOnlyList<TextContent> textContents = await hunyuanChatCompletionService.GetTextContentsAsync("Prompt", executionSettings);

        Assert.NotNull(textContents);
        Assert.True(textContents.Count > 0);

        byte[]? requestContent = this._messageHandlerStub.RequestContent;

        Assert.NotNull(requestContent);

        JsonElement content = JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(requestContent));

        Assert.Equal(TestConstants.FakeModel, content.GetProperty("Model").GetString());
        Assert.Equal("system", content.GetProperty("Messages")[0].GetProperty("Role").GetString());
        Assert.Equal("Assistant is a large language model.", content.GetProperty("Messages")[0].GetProperty("Content").GetString());
        Assert.Equal("user", content.GetProperty("Messages")[1].GetProperty("Role").GetString());
        Assert.Equal("Prompt", content.GetProperty("Messages")[1].GetProperty("Content").GetString());
        Assert.False(content.GetProperty("Stream").GetBoolean());
        Assert.True(content.GetProperty("StreamModeration").GetBoolean());
        Assert.True(content.GetProperty("EnableEnhancement").GetBoolean());
        Assert.Equal(0.3f, content.GetProperty("TopP").GetSingle());
        Assert.Equal(0.9f, content.GetProperty("Temperature").GetSingle());
    }


    [Fact]
    public async Task GetStreamingTextContentsWorksCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        using MemoryStream stream = new(Encoding.UTF8.GetBytes(HunyuanTestHelper.GetTestResponse("chat_generation_test_stream_response.txt")));

        HttpResponseMessage responseMessage = new(HttpStatusCode.OK)
        {
            Content = new StreamContent(stream),
        };

        responseMessage.Headers.Add("X-TC-RequestId", Guid.NewGuid().ToString());
        responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");

        this._messageHandlerStub.ResponseToReturn = responseMessage;

        StringBuilder contentBuilder = new();

        await foreach (StreamingTextContent chunk in hunyuanChatCompletionService.GetStreamingTextContentsAsync("Prompt"))
        {
            contentBuilder.Append(chunk.Text);

            Assert.Equal(TestConstants.FakeModel, chunk.ModelId);
            Assert.IsType<HunyuanChatCompletionMetadata>(chunk.Metadata);

            HunyuanChatCompletionMetadata? metadata = chunk.Metadata as HunyuanChatCompletionMetadata;

            Assert.NotNull(metadata);

            if (string.IsNullOrEmpty(metadata!.FinishReason))
            {
                Assert.NotNull(metadata.Delta);
                Assert.Equal(string.Empty, metadata.Delta.Role);
                Assert.Equal(chunk.Text, metadata.Delta.Content);
            }
            else
            {
                Assert.Equal("assistant", metadata.Delta!.Role);
            }
        }

        Assert.Equal("Hey there", contentBuilder.ToString());
    }
    public void Dispose()
    {
        this._messageHandlerStub.Dispose();
        this._httpClient.Dispose();
    }
}