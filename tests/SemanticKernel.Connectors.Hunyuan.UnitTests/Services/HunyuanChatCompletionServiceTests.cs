using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;

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
        this._httpClient = new HttpClient(_messageHandlerStub, false);
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWorksCorrectly(bool includeLoggerFactory)
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = includeLoggerFactory
            ? new HunyuanChatCompletionService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object)
            : new HunyuanChatCompletionService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, TestConstants.FakeRegion, loggerFactory: this._mockLoggerFactory.Object);

        Assert.NotNull(hunyuanChatCompletionService);
        Assert.Equal(TestConstants.FakeModel, hunyuanChatCompletionService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithTimeoutWorksCorrectly(bool includeLoggerFactory)
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = includeLoggerFactory
            ? new HunyuanChatCompletionService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 100, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object)
            : new HunyuanChatCompletionService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 100, TestConstants.FakeRegion, loggerFactory: this._mockLoggerFactory.Object);

        Assert.NotNull(hunyuanChatCompletionService);
        Assert.Equal(TestConstants.FakeModel, hunyuanChatCompletionService.Attributes["ModelId"]);
    }

    [Fact]
    public async Task GetChatMessageContentsWorksCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        IReadOnlyList<ChatMessageContent> chatMessageContents = await hunyuanChatCompletionService.GetChatMessageContentsAsync([new ChatMessageContent(AuthorRole.User, "Prompt")]);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);
        Assert.Equal("你好！很高兴为您提供帮助，请问您有什么问题？", chatMessageContents[0].Content);
    }

    [Fact]
    public async Task GetChatMessageContentHandlesSettingCorrectlyAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

        HunyuanTestHelper.SetTestHttpClient(hunyuanChatCompletionService, this._httpClient);

        HunyuanPromptExecutionSettings executionSettings = new()
        {
            EnableEnhancement = true,
            Temperature = 0.9f,
            StreamModeration = true,
            Stream = false,
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
    public async Task ShouldHandleMetadataAsync()
    {
        HunyuanChatCompletionService hunyuanChatCompletionService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object);

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

    public void Dispose()
    {
        this._messageHandlerStub.Dispose();
        this._httpClient.Dispose();
    }
}