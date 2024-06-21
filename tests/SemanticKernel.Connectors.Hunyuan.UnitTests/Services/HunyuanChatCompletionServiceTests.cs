// Copyright (c) IdeaTech. All rights reserved.

namespace SemanticKernel.Connectors.Hunyuan.UnitTests.Services;

public sealed class HunyuanChatCompletionServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;

    private readonly HttpClient _httpClient;

    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public HunyuanChatCompletionServiceTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(HunyuanTestHelper.GetTestResponse("chat_completion_test_response.json"));
        this._httpClient = new HttpClient(this._messageHandlerStub, false);
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWorksCorrectly(bool includeLoggerFactory)
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = includeLoggerFactory
            ? new HunyuanChatCompletionService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object)
            : new HunyuanChatCompletionService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, loggerFactory: this._mockLoggerFactory.Object);

        Assert.NotNull(hunyuanChatCompletionService);
        Assert.Equal(TestConstants.FakeModel, hunyuanChatCompletionService.Attributes["ModelId"]);
    }

    [Fact]
    public async Task GetChatMessageContentsWorksCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        IReadOnlyList<ChatMessageContent> chatMessageContents = await hunyuanChatCompletionService.GetChatMessageContentsAsync([new ChatMessageContent(AuthorRole.User, "Prompt")]);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);
        Assert.Equal("你好！很高兴为您提供帮助，请问您有什么问题？", chatMessageContents[0].Content);
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
            TopP = 0.3f
        };

        ChatHistory chatHistory = [];
        chatHistory.AddUserMessage("Prompt");

        IReadOnlyList<ChatMessageContent> chatMessageContents = await hunyuanChatCompletionService.GetChatMessageContentsAsync(chatHistory, executionSettings);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);

        byte[]? requestContent = this._messageHandlerStub.RequestContent;

        Assert.NotNull(requestContent);

        JsonElement content = JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(requestContent));

        Assert.Equal(TestConstants.FakeModel, content.GetProperty("Model").GetString());
        Assert.Equal("user", content.GetProperty("Messages")[0].GetProperty("Role").GetString());
        Assert.Equal("Prompt", content.GetProperty("Messages")[0].GetProperty("Content").GetString());
        Assert.False(content.GetProperty("Stream").GetBoolean());
        Assert.True(content.GetProperty("StreamModeration").GetBoolean());
        Assert.True(content.GetProperty("EnableEnhancement").GetBoolean());
        Assert.Equal(0.3f, content.GetProperty("TopP").GetSingle());
        Assert.Equal(0.9f, content.GetProperty("Temperature").GetSingle());
    }

    [Fact]
    public async Task GetChatMessageContentHandlesNullUsageCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(HunyuanTestHelper.GetTestResponse("chat_completion_test_null_values_response.json"));

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        IReadOnlyList<ChatMessageContent> chatMessageContents = await hunyuanChatCompletionService.GetChatMessageContentsAsync([new ChatMessageContent(AuthorRole.User, "Prompt")]);

        Assert.NotNull(chatMessageContents);
        Assert.Equal(AuthorRole.Assistant, chatMessageContents[0].Role);
        Assert.NotNull(chatMessageContents[0].Metadata);
        Assert.Null(chatMessageContents[0].Metadata!["Usage"]);
    }

    [Fact]
    public async Task ShouldHandleMetadataAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        IReadOnlyList<ChatMessageContent> chatMessageContents = await hunyuanChatCompletionService.GetChatMessageContentsAsync([new ChatMessageContent(AuthorRole.User, "Prompt")]);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);

        ChatMessageContent content = chatMessageContents.SingleOrDefault()!;

        Assert.NotNull(content);
        Assert.Equal(TestConstants.FakeModel, content.ModelId);
        Assert.IsType<HunyuanChatCompletionMetadata>(content.Metadata);

        HunyuanChatCompletionMetadata? metadata = content.Metadata as HunyuanChatCompletionMetadata;

        Assert.Equal("你好！很高兴为您提供帮助，请问您有什么问题？", content.Content);
        Assert.Equal("22222222-d59f-42a1-90ed-b4956fb14650", metadata!.Id);
        Assert.Equal("11111111-d59f-42a1-90ed-b4956fb14650", metadata!.RequestId);
        Assert.Equal("以上内容为AI生成，不代表开发者立场，请勿删除或修改本标记", metadata.Note);
        Assert.Equal(1718297402L, metadata.Created);
        Assert.Equal("stop", metadata.FinishReason);
        Assert.Equal(3, metadata.Usage!.PromptTokens);
        Assert.Equal(12, metadata.Usage.CompletionTokens);
        Assert.Equal(15, metadata.Usage.TotalTokens);
        Assert.Equal(3597823239L, metadata.ErrorMsg!.Code);
        Assert.Equal("success", metadata.ErrorMsg!.Msg);
    }

    [Fact]
    public async Task GetStreamingChatMessageContentsWorksCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        await using MemoryStream stream = new(Encoding.UTF8.GetBytes(HunyuanTestHelper.GetTestResponse("chat_generation_test_stream_response.txt")));

        HttpResponseMessage responseMessage = new(HttpStatusCode.OK)
        {
            Content = new StreamContent(stream),
        };

        responseMessage.Headers.Add("X-TC-RequestId", Guid.NewGuid().ToString());
        responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");

        this._messageHandlerStub.ResponseToReturn = responseMessage;

        ChatHistory history = new("You are an useful AI Assistant");
        history.AddUserMessage("Prompt");

        HunyuanPromptExecutionSettings executionSettings = new()
        {
            EnableEnhancement = true,
            Temperature = 0.2f,
            StreamModeration = true,
            TopP = 0.5f
        };

        StringBuilder contentBuilder = new();

        await foreach (StreamingChatMessageContent chunk in hunyuanChatCompletionService.GetStreamingChatMessageContentsAsync(history, executionSettings))
        {
            contentBuilder.Append(chunk.Content);

            Assert.Equal(TestConstants.FakeModel, chunk.ModelId);
            Assert.IsType<HunyuanChatCompletionMetadata>(chunk.Metadata);

            HunyuanChatCompletionMetadata? metadata = chunk.Metadata as HunyuanChatCompletionMetadata;

            Assert.NotNull(metadata);

            if (string.IsNullOrEmpty(metadata!.FinishReason))
            {
                Assert.NotNull(metadata.Delta);
                Assert.Equal(string.Empty, metadata.Delta.Role);
                Assert.Equal(chunk.Content, metadata.Delta.Content);
            }
            else
            {
                Assert.Equal("assistant", metadata.Delta!.Role);
            }
        }

        Assert.Equal("Hey there", contentBuilder.ToString());
    }

    [Fact]
    public async Task GetStreamingChatMessageContentHandlesSettingCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        HunyuanPromptExecutionSettings executionSettings = new()
        {
            EnableEnhancement = true,
            Temperature = 0.2f,
            StreamModeration = true,
            TopP = 0.5f
        };

        await using MemoryStream stream = new(Encoding.UTF8.GetBytes(HunyuanTestHelper.GetTestResponse("chat_generation_test_stream_response.txt")));

        HttpResponseMessage responseMessage = new(HttpStatusCode.OK)
        {
            Content = new StreamContent(stream),
        };

        responseMessage.Headers.Add("X-TC-RequestId", Guid.NewGuid().ToString());
        responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");

        this._messageHandlerStub.ResponseToReturn = responseMessage;

        ChatHistory history = new("You are an useful AI Assistant");
        history.AddUserMessage("Prompt");

        IReadOnlyList<StreamingChatMessageContent> chatMessageContents = await hunyuanChatCompletionService.GetStreamingChatMessageContentsAsync(history, executionSettings).ToListAsync();

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);

        byte[]? requestContent = this._messageHandlerStub.RequestContent;

        Assert.NotNull(requestContent);

        JsonElement content = JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(requestContent));

        Assert.Equal(TestConstants.FakeModel, content.GetProperty("Model").GetString());
        Assert.Equal("You are an useful AI Assistant", content.GetProperty("Messages")[0].GetProperty("Content").GetString());
        Assert.Equal("system", content.GetProperty("Messages")[0].GetProperty("Role").GetString());
        Assert.Equal("Prompt", content.GetProperty("Messages")[1].GetProperty("Content").GetString());
        Assert.Equal("user", content.GetProperty("Messages")[1].GetProperty("Role").GetString());

        Assert.True(content.GetProperty("Stream").GetBoolean());
        Assert.True(content.GetProperty("StreamModeration").GetBoolean());
        Assert.True(content.GetProperty("EnableEnhancement").GetBoolean());
        Assert.Equal(0.5f, content.GetProperty("TopP").GetSingle());
        Assert.Equal(0.2f, content.GetProperty("Temperature").GetSingle());
    }

    [Fact]
    public async Task GetStreamingChatMessageContentsShouldThrowAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        await using MemoryStream stream = new(Encoding.UTF8.GetBytes(HunyuanTestHelper.GetTestResponse("chat_generation_test_stream_invalid_response.txt")));

        HttpResponseMessage responseMessage = new(HttpStatusCode.OK)
        {
            Content = new StreamContent(stream),
        };

        responseMessage.Headers.Add("X-TC-RequestId", Guid.NewGuid().ToString());
        responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/event-stream");

        this._messageHandlerStub.ResponseToReturn = responseMessage;

        ChatHistory history = new("You are an useful AI Assistant");
        history.AddUserMessage("Prompt");

        KernelException exception = await Assert.ThrowsAsync<KernelException>(async () => await hunyuanChatCompletionService.GetStreamingChatMessageContentsAsync(history).ToListAsync());

        Assert.Equal("Unexpected response from model", exception.Message);
    }

    public void Dispose()
    {
        this._messageHandlerStub.Dispose();
        this._httpClient.Dispose();
    }
}
