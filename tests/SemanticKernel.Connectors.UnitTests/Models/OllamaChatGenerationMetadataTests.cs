namespace SemanticKernel.Connectors.UnitTests;

public class OllamaChatGenerationMetadataTests
{
    private static readonly Dictionary<string, object?> TestDictionaryData = new()
    {
        [nameof(OllamaChatGenerationMetadata.PromptEvalCount)] = 33,
        [nameof(OllamaChatGenerationMetadata.PromptEvalDuration)] = 78567900L,
        [nameof(OllamaChatGenerationMetadata.EvalCount)] = 554,
        [nameof(OllamaChatGenerationMetadata.DoneReason)] = "length",
        [nameof(OllamaChatGenerationMetadata.TotalDuration)] = 374486800L,
        [nameof(OllamaChatGenerationMetadata.LoadDuration)] = 568785000L,
        [nameof(OllamaChatGenerationMetadata.EvalDuration)] = 239800L,
        [nameof(OllamaChatGenerationMetadata.Done)] = false,
        [nameof(OllamaChatGenerationMetadata.CreatedAt)] = new DateTimeOffset(2024, 6, 5, 12, 34, 56, TimeSpan.Zero)
    };

    private static readonly IReadOnlyDictionary<string, object?> ReadOnlyTestDictionaryData = TestDictionaryData.AsReadOnly();

    [Fact]
    public void FromDictionaryWithCustomReadOnlyDictionaryShouldWorkCorrectly()
    {
        OllamaChatGenerationMetadata metadata = OllamaChatGenerationMetadata.FromDictionary(new MockIReadOnlyDictionary(TestDictionaryData));

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithDictionaryShouldWorkCorrectly()
    {
        OllamaChatGenerationMetadata metadata = OllamaChatGenerationMetadata.FromDictionary(TestDictionaryData);

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithReadOnlyDictionaryShouldWorkCorrectly()
    {
        OllamaChatGenerationMetadata metadata = OllamaChatGenerationMetadata.FromDictionary(ReadOnlyTestDictionaryData);

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithMetadataShouldWorkCorrectly()
    {
        OllamaChatGenerationMetadata metadataSource = OllamaChatGenerationMetadata.FromDictionary(ReadOnlyTestDictionaryData);

        OllamaChatGenerationMetadata metadata = OllamaChatGenerationMetadata.FromDictionary(metadataSource);

        AssertMetaData(metadata);
    }

    [Fact]
    public void GetValueFromDictionaryWhenKeyNotExistsShouldReturnNull()
    {
        OllamaChatGenerationMetadata metadata = OllamaChatGenerationMetadata.FromDictionary(new Dictionary<string, object?>());

        Assert.NotNull(metadata);

        Assert.Null(metadata.PromptEvalCount);
        Assert.Null(metadata.PromptEvalDuration);
        Assert.Null(metadata.EvalCount);
        Assert.Null(metadata.DoneReason);
        Assert.Null(metadata.TotalDuration);
        Assert.Null(metadata.LoadDuration);
        Assert.Null(metadata.EvalDuration);
        Assert.Null(metadata.Done);
        Assert.Null(metadata.CreatedAt);
    }

    private static void AssertMetaData(OllamaChatGenerationMetadata metadata)
    {
        Assert.NotNull(metadata);

        Assert.Equal(33, metadata.PromptEvalCount);
        Assert.Equal(78567900, metadata.PromptEvalDuration);
        Assert.Equal(554, metadata.EvalCount);
        Assert.Equal("length", metadata.DoneReason);
        Assert.Equal(374486800, metadata.TotalDuration);
        Assert.Equal(568785000, metadata.LoadDuration);
        Assert.Equal(239800, metadata.EvalDuration);
        Assert.Equal(false, metadata.Done);
        Assert.Equal(new DateTimeOffset(2024, 6, 5, 12, 34, 56, TimeSpan.Zero), metadata.CreatedAt);
    }

    [Fact]
    public void FromDictionaryShouldThrowWithNullDictionary()
    {
        Assert.Throws<ArgumentNullException>(() => OllamaChatGenerationMetadata.FromDictionary(null));
    }
}