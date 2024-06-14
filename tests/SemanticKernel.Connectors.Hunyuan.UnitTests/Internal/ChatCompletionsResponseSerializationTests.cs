namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

public sealed class ChatCompletionsResponseSerializationTests
{

    [Fact]
    public void ChatCompletionsResponseSerializationWithNullShouldReturnNull()
    {
        JsonDocument document = JsonDocument.Parse("null");

        ChatCompletionsResponse? response = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponse(document.RootElement);

        Assert.Null(response);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationShouldWorkCorrectly()
    {
        string payload = """
        {
            "RequestId": "11111111-d59f-42a1-90ed-b4956fb14650",
            "Note": "以上内容为AI生成，不代表开发者立场，请勿删除或修改本标记",
            "Choices": [
                {
                    "Delta": {
                        "Role": "assistant",
                        "Content": "你好！很高兴为您提供帮助，请问您有什么问题？"
                    },
                    "FinishReason": "stop"
                }
            ],
            "Created": 1718297402,
            "Id": "22222222-d59f-42a1-90ed-b4956fb14650",
            "Usage": {
                "PromptTokens": 3,
                "CompletionTokens": 12,
                "TotalTokens": 15
            },
            "ErrorMsg":{
                "Code": 123545433,
                "Msg": "success"
            }
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        ChatCompletionsResponse? response = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponse(document.RootElement);

        Assert.NotNull(response);
        Assert.NotNull(response.Choices);
        Assert.NotEmpty(response.Choices);
        Assert.NotNull(response.Choices[0].Delta);
        Assert.Equal("assistant", response.Choices[0].Delta.Role);
        Assert.Equal("你好！很高兴为您提供帮助，请问您有什么问题？", response.Choices[0].Delta.Content);
        Assert.Equal(1718297402, response.Created);
        Assert.Equal("22222222-d59f-42a1-90ed-b4956fb14650", response.Id);
        Assert.NotNull(response.Usage);
        Assert.Equal(3, response.Usage.PromptTokens);
        Assert.Equal(12, response.Usage.CompletionTokens);
        Assert.Equal(15, response.Usage.TotalTokens);
        Assert.NotNull(response.ErrorMsg);
        Assert.Equal(123545433, response.ErrorMsg.Code);
        Assert.Equal("success", response.ErrorMsg.Msg);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationWithNullValuesShouldWorkCorrectly()
    {
        const string payload = """
        {
            "RequestId": "11111111-d59f-42a1-90ed-b4956fb14650",
            "Note": "以上内容为AI生成，不代表开发者立场，请勿删除或修改本标记",
            "Choices": null,
            "Created": 1718297402,
            "Id": "22222222-d59f-42a1-90ed-b4956fb14650",
            "Usage": null,
            "ErrorMsg":null
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        ChatCompletionsResponse? response = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponse(document.RootElement);

        Assert.NotNull(response);
        Assert.NotNull(response.Choices);
        Assert.Empty(response.Choices);
        Assert.Null(response.Usage);
        Assert.Null(response.ErrorMsg);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationChoicesWithNullShouldReturnNull()
    {
        JsonDocument document = JsonDocument.Parse("null");

        Choice? choice = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseChoice(document.RootElement);

        Assert.Null(choice);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationChoicesFinishReasonShouldNull()
    {
        const string payload = """
        {
            "Delta": {
                "Role": "assistant",
                "Content": "你好！很高兴为您提供帮助，请问您有什么问题？"
            },
            "FinishReason": null,
            "XXX":"..."
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        Choice? choice = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseChoice(document.RootElement);

        Assert.NotNull(choice);
        Assert.Null(choice.FinishReason);
    }


    [Fact]
    public void ChatCompletionsResponseSerializationChoicesDeltaShouldWorkCorrectly()
    {
        const string payload = """
        {
            "Role": "assistant",
            "Content": "你好！很高兴为您提供帮助，请问您有什么问题？",
            "XXX":"..."
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        Delta? delta = ChatCompletionsResponseSerialization.DeserializeChatCompletionResponseChoiceDelta(document.RootElement);

        Assert.NotNull(delta);
        Assert.Equal("assistant", delta.Role);
        Assert.Equal("你好！很高兴为您提供帮助，请问您有什么问题？", delta.Content);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationChoicesDeltaShouldNull()
    {
        JsonDocument document = JsonDocument.Parse("null");

        Delta? delta = ChatCompletionsResponseSerialization.DeserializeChatCompletionResponseChoiceDelta(document.RootElement);

        Assert.Null(delta);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationChoicesDeltaPropertiesShouldNull()
    {
        const string payload = """
        {
            "Role": null,
            "Content": null
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        Delta? delta = ChatCompletionsResponseSerialization.DeserializeChatCompletionResponseChoiceDelta(document.RootElement);

        Assert.NotNull(delta);
        Assert.Empty(delta.Role);
        Assert.Empty(delta.Content);
    }


    [Fact]
    public void ChatCompletionsResponseSerializationUsageShouldWorkCorrectly()
    {
        const string payload = """
        {
            "PromptTokens": 1,
            "CompletionTokens": 2,
            "TotalTokens": 3,
            "XXX":"..."
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        Usage? usage = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseUsage(document.RootElement);

        Assert.NotNull(usage);
        Assert.Equal(1, usage.PromptTokens);
        Assert.Equal(2, usage.CompletionTokens);
        Assert.Equal(3, usage.TotalTokens);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationUsageShouldNull()
    {
        JsonDocument document = JsonDocument.Parse("null");

        Usage? usage = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseUsage(document.RootElement);

        Assert.Null(usage);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationUsagePropertiesShouldDefault()
    {
        const string payload = """
        {
            "PromptTokens": null,
            "CompletionTokens": null,
            "TotalTokens": null
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        Usage? usage = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseUsage(document.RootElement);

        Assert.NotNull(usage);
        Assert.Equal(0, usage.PromptTokens);
        Assert.Equal(0, usage.CompletionTokens);
        Assert.Equal(0, usage.TotalTokens);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationErrorMsgShouldWorkCorrectly()
    {
        const string payload = """
        {
            "Code": 133445566,
            "Msg": "success",
            "XXX":"..."
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        ErrorMsg? errorMsg = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseErrorMsg(document.RootElement);

        Assert.NotNull(errorMsg);
        Assert.Equal(133445566, errorMsg.Code);
        Assert.Equal("success", errorMsg.Msg);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationErrorMsgShouldNull()
    {
        JsonDocument document = JsonDocument.Parse("null");

        ErrorMsg? errorMsg = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseErrorMsg(document.RootElement);

        Assert.Null(errorMsg);
    }

    [Fact]
    public void ChatCompletionsResponseSerializationErrorMsgPropertiesShouldNull()
    {
        const string payload = """
        {
            "Code": null,
            "Msg": null
        }
        """;

        JsonDocument document = JsonDocument.Parse(payload);

        ErrorMsg? errorMsg = ChatCompletionsResponseSerialization.DeserializeChatCompletionsResponseErrorMsg(document.RootElement);

        Assert.NotNull(errorMsg);
        Assert.Equal(0, errorMsg.Code);
        Assert.Empty(errorMsg.Msg);
    }
}
