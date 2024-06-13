namespace SemanticKernel.Connectors.Hunyuan.UnitTests.Models;

public class HunyuanChatCompletionMetadataTests
{
    private static readonly Dictionary<string, object?> TestDictionaryData = new()
    {
        [nameof(HunyuanChatCompletionMetadata.Usage)] = new Usage
        {
            PromptTokens = 10,
            CompletionTokens = 25,
            TotalTokens = 35
        },
        [nameof(HunyuanChatCompletionMetadata.Note)] = "以上内容为AI生成，不代表开发者立场，请勿删除或修改本标记",
        [nameof(HunyuanChatCompletionMetadata.Id)] = "ed03567e-02d7-460a-aafe-55c8ed1bc286",
        [nameof(HunyuanChatCompletionMetadata.ErrorMsg)] = new ErrorMsg
        {
            Code = 123456789L,
            Msg = "success"
        },
        [nameof(HunyuanChatCompletionMetadata.RequestId)] = "daf9170f-e3ce-4d3b-b8e4-7ec6b4e72577",
        [nameof(HunyuanChatCompletionMetadata.FinishReason)] = "stop",
        [nameof(HunyuanChatCompletionMetadata.Delta)] = new Delta
        {
            Role = "assistant",
            Content = "How"
        },
        [nameof(HunyuanChatCompletionMetadata.Created)] = 1352468L
    };

    private static readonly IReadOnlyDictionary<string, object?> ReadOnlyTestDictionaryData = TestDictionaryData.AsReadOnly();

    [Fact]
    public void FromDictionaryWithCustomReadOnlyDictionaryShouldWorkCorrectly()
    {
        HunyuanChatCompletionMetadata metadata = HunyuanChatCompletionMetadata.FromDictionary(new MockIReadOnlyDictionary(TestDictionaryData));

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithDictionaryShouldWorkCorrectly()
    {
        HunyuanChatCompletionMetadata metadata = HunyuanChatCompletionMetadata.FromDictionary(TestDictionaryData);

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithReadOnlyDictionaryShouldWorkCorrectly()
    {
        HunyuanChatCompletionMetadata metadata = HunyuanChatCompletionMetadata.FromDictionary(ReadOnlyTestDictionaryData);

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithMetadataShouldWorkCorrectly()
    {
        HunyuanChatCompletionMetadata metadataSource = HunyuanChatCompletionMetadata.FromDictionary(ReadOnlyTestDictionaryData);

        HunyuanChatCompletionMetadata metadata = HunyuanChatCompletionMetadata.FromDictionary(metadataSource);

        AssertMetaData(metadata);
    }

    [Fact]
    public void GetValueFromDictionaryWhenKeyNotExistsShouldReturnNull()
    {
        HunyuanChatCompletionMetadata metadata = HunyuanChatCompletionMetadata.FromDictionary(new Dictionary<string, object?>());

        Assert.NotNull(metadata);

        Assert.Null(metadata.Usage);
        Assert.Null(metadata.Note);
        Assert.Null(metadata.Id);
        Assert.Null(metadata.ErrorMsg);
        Assert.Null(metadata.RequestId);
        Assert.Null(metadata.FinishReason);
        Assert.Null(metadata.Delta);
    }

    private static void AssertMetaData(HunyuanChatCompletionMetadata metadata)
    {
        Assert.NotNull(metadata);

        Assert.Equal(10, metadata!.Usage!.PromptTokens);
        Assert.Equal(25, metadata.Usage.CompletionTokens);
        Assert.Equal(35, metadata.Usage.TotalTokens);
        Assert.Equal("以上内容为AI生成，不代表开发者立场，请勿删除或修改本标记", metadata.Note);
        Assert.Equal("ed03567e-02d7-460a-aafe-55c8ed1bc286", metadata.Id);
        Assert.Equal(123456789L, metadata!.ErrorMsg!.Code);
        Assert.Equal("success", metadata.ErrorMsg.Msg);
        Assert.Equal("daf9170f-e3ce-4d3b-b8e4-7ec6b4e72577", metadata.RequestId);
        Assert.Equal("stop", metadata.FinishReason);
        Assert.Equal("How", metadata.Delta!.Content);
        Assert.Equal("assistant", metadata.Delta.Role);
        Assert.Equal(1352468L, metadata.Created);
    }

    [Fact]
    public void FromDictionaryWithNullDictionaryShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => HunyuanChatCompletionMetadata.FromDictionary(null));
    }
}