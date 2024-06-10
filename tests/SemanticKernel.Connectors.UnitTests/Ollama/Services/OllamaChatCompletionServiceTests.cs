namespace SemanticKernel.Connectors.UnitTests.Ollama.Services;

public sealed class OllamaChatCompletionServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public OllamaChatCompletionServiceTests()
    {
        _messageHandlerStub = new HttpMessageHandlerStub();
        _messageHandlerStub.ResponseToReturn.Content = new StringContent(OllamaTestHelper.GetTestResponse("chat_completion_test_response.json"));
        _httpClient = new HttpClient(_messageHandlerStub, false)
        {
            BaseAddress = TestConstants.FakeUri
        };
        _mockLoggerFactory = new Mock<ILoggerFactory>();
    }

    #region Constructors

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithUriStringWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaChatCompletionService ollamaChatCompletionService = includeLoggerFactory
            ? new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeUriString, loggerFactory: _mockLoggerFactory.Object)
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
            ? new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeUri, loggerFactory: _mockLoggerFactory.Object)
            : new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeUri);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWithHttpClientWorksCorrectly(bool includeLoggerFactory)
    {
        OllamaChatCompletionService ollamaTextGenerationService = includeLoggerFactory
            ? new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeHttpClient, loggerFactory: _mockLoggerFactory.Object)
            : new OllamaChatCompletionService(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Assert.NotNull(ollamaTextGenerationService);
        Assert.Equal(TestConstants.FakeModel, ollamaTextGenerationService.Attributes["ModelId"]);
    }

    #endregion

    [Fact]
    public async Task GetChatMessageContentsWorksCorrectlyAsync()
    {
        OllamaChatCompletionService ollamaChatCompletionService = new(TestConstants.FakeModel, _httpClient);

        IReadOnlyList<ChatMessageContent> chatMessageContents = await ollamaChatCompletionService.GetChatMessageContentsAsync([new ChatMessageContent(AuthorRole.User, "Prompt")]);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);
        Assert.Equal("This is a test chat completion response", chatMessageContents[0].Content);
    }

    [Fact]
    public async Task GetChatMessageContentHandlesSettingCorrectlyAsync()
    {
        OllamaChatCompletionService ollamaChatCompletionService = new(TestConstants.FakeModel, _httpClient);

        OllamaPromptExecutionSettings executionSettings = new()
        {
            MaxTokens = 256,
            Temperature = 0.1,
            TopP = 0.4,
            TopK = 66,
            FrequencyPenalty = 1.1,
            PresencePenalty = 1.5,
            Seed = 10,
            KeepAlive = 123,
            Stop = ["stop_sequence"]
        };

        ChatHistory history = new("You are an useful AI Assistant");
        history.AddUserMessage("Prompt");

        _messageHandlerStub.ResponseToReturn = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(OllamaTestHelper.GetTestResponse("chat_completion_test_response.json"))
        };

        IReadOnlyList<ChatMessageContent> chatMessageContents = await ollamaChatCompletionService.GetChatMessageContentsAsync(history, executionSettings);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);

        byte[]? requestContent = _messageHandlerStub.RequestContent;

        Assert.NotNull(requestContent);

        JsonElement content = JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(requestContent));

        Assert.Equal(TestConstants.FakeModel, content.GetProperty("model").GetString());
        Assert.Equal("You are an useful AI Assistant", content.GetProperty("messages")[0].GetProperty("content").GetString());
        Assert.Equal("Prompt", content.GetProperty("messages")[1].GetProperty("content").GetString());
        Assert.Equal(123, content.GetProperty("keep_alive").GetInt32());
        Assert.Equal(256, content.GetProperty("options").GetProperty("num_ctx").GetInt32());
        Assert.Equal(0.1, content.GetProperty("options").GetProperty("temperature").GetDouble());
        Assert.Equal(0.4, content.GetProperty("options").GetProperty("top_p").GetDouble());
        Assert.Equal(66, content.GetProperty("options").GetProperty("top_k").GetInt32());
        Assert.Equal(1.5, content.GetProperty("options").GetProperty("presence_penalty").GetDouble());
        Assert.Equal(1.1, content.GetProperty("options").GetProperty("frequency_penalty").GetDouble());
        Assert.Equal(10, content.GetProperty("options").GetProperty("seed").GetInt32());
        Assert.Equal("stop_sequence", content.GetProperty("options").GetProperty("stop")[0].GetString());
    }

    [Fact]
    public async Task ShouldHandleMetadataAsync()
    {
        OllamaChatCompletionService ollamaChatCompletionService = new(TestConstants.FakeModel, _httpClient);

        IReadOnlyList<ChatMessageContent> chatMessageContents = await ollamaChatCompletionService.GetChatMessageContentsAsync([new ChatMessageContent(AuthorRole.User, "Prompt")]);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);

        ChatMessageContent content = chatMessageContents.SingleOrDefault()!;

        Assert.NotNull(content);
        Assert.Equal("llama3", content.ModelId);
        Assert.IsType<OllamaChatGenerationMetadata>(content.Metadata);

        OllamaChatGenerationMetadata? metadata = content.Metadata as OllamaChatGenerationMetadata;

        Assert.Equal("This is a test chat completion response", content.Content);
        Assert.Equal(4684546714, metadata!.TotalDuration);
        Assert.Equal(776435, metadata.LoadDuration);
        Assert.Equal(0, metadata.PromptEvalCount);
        Assert.Equal(182528000, metadata.PromptEvalDuration);
        Assert.Equal(27, metadata.EvalCount);
        Assert.Equal(4350813000, metadata.EvalDuration);
        Assert.Equal("stop", metadata.DoneReason);

        Assert.True(metadata.Done);

        _ = DateTimeOffset.TryParse("2024-06-10T09:28:29.2747503+00:00", out DateTimeOffset date);
        Assert.True(metadata.CreatedAt == date);
    }

    [Fact]
    public async Task GetStreamingChatMessageContentsWorksCorrectlyAsync()
    {
        OllamaChatCompletionService ollamaChatCompletionService = new(TestConstants.FakeModel, _httpClient);

        using MemoryStream stream = new(Encoding.UTF8.GetBytes(OllamaTestHelper.GetTestResponse("chat_generation_test_stream_response.txt")));

        _messageHandlerStub.ResponseToReturn = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StreamContent(stream)
        };

        StringBuilder contentBuilder = new();

        await foreach (var chunk in ollamaChatCompletionService.GetStreamingChatMessageContentsAsync("Prompt"))
        {
            contentBuilder.Append(chunk.Content);

            Assert.Equal("llama3", chunk.ModelId);
            Assert.IsType<OllamaChatGenerationMetadata>(chunk.Metadata);

            OllamaChatGenerationMetadata? metadata = chunk.Metadata as OllamaChatGenerationMetadata;

            if (metadata!.Done.HasValue && metadata.Done.Value)
            {
                Assert.Equal(string.Empty, chunk.Content);
                Assert.Equal(4392904240, metadata.TotalDuration);
                Assert.Equal(722247, metadata.LoadDuration);
                Assert.Equal(0, metadata.PromptEvalCount);
                Assert.Equal(187628000, metadata.PromptEvalDuration);
                Assert.Equal(26, metadata.EvalCount);
                Assert.Equal(4051593000, metadata.EvalDuration);
                Assert.Equal("stop", metadata.DoneReason);

                _ = DateTimeOffset.TryParse("2024-06-10T09:29:41.3487684+00:00", out DateTimeOffset date);
                Assert.True(metadata.CreatedAt == date);
            }
        }

        Assert.Equal("Hello! It's nice to meet you. Is there something I can help you with, or would you like to chat?", contentBuilder.ToString());
    }

    [Fact]
    public async Task GetChatMessageContentsWorksCorrectlyWithInvalidChatCompletionResponseAsync()
    {
        OllamaChatCompletionService ollamaChatCompletionService = new(TestConstants.FakeModel, _httpClient);

        _messageHandlerStub.ResponseToReturn = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(OllamaTestHelper.GetTestResponse("chat_completion_test_invalid_response.json"))
        };

        IReadOnlyList<ChatMessageContent> chatMessageContents = await ollamaChatCompletionService.GetChatMessageContentsAsync([new ChatMessageContent(AuthorRole.User, "Prompt")]);

        Assert.NotNull(chatMessageContents);
        Assert.True(chatMessageContents.Count > 0);
        Assert.Equal(AuthorRole.Assistant, chatMessageContents[0].Role);
    }

    [Fact]
    public async Task GetStreamingChatMessageContentsWorksCorrectlyWithInvalidChatCompletionResponseAsync()
    {
        OllamaChatCompletionService ollamaChatCompletionService = new(TestConstants.FakeModel, _httpClient);

        using MemoryStream stream = new(Encoding.UTF8.GetBytes(OllamaTestHelper.GetTestResponse("chat_generation_test_stream_invalid_response.txt")));

        _messageHandlerStub.ResponseToReturn = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StreamContent(stream)
        };

        await foreach (var chunk in ollamaChatCompletionService.GetStreamingChatMessageContentsAsync("Prompt"))
        {
            bool? done = chunk.Metadata![nameof(OllamaChatGenerationMetadata.Done)] as bool?;

            if (done.HasValue && done.Value)
            {
                Assert.Equal(AuthorRole.Assistant, chunk.Role);
            }
            else
            {
                Assert.Null(chunk.Role);
            }
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _messageHandlerStub.Dispose();
    }
}
