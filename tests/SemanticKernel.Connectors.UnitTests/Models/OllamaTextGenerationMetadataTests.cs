namespace SemanticKernel.Connectors.UnitTests;

public class OllamaTextGenerationMetadataTests
{
    private static readonly long[] context = [1L, 2L, 4L];

    private static readonly Dictionary<string, object?> TestDictionaryData =
    new()
    {
        [nameof(OllamaTextGenerationMetadata.PromptEvalCount)] = 16,
        [nameof(OllamaTextGenerationMetadata.PromptEvalDuration)] = 665900L,
        [nameof(OllamaTextGenerationMetadata.EvalCount)] = 87,
        [nameof(OllamaTextGenerationMetadata.DoneReason)] = "stop",
        [nameof(OllamaTextGenerationMetadata.TotalDuration)] = 5678600L,
        [nameof(OllamaTextGenerationMetadata.LoadDuration)] = 17800L,
        [nameof(OllamaTextGenerationMetadata.EvalDuration)] = 86700L,
        [nameof(OllamaTextGenerationMetadata.Done)] = true,
        [nameof(OllamaTextGenerationMetadata.CreatedAt)] = new DateTimeOffset(2024, 6, 5, 13, 38, 21, TimeSpan.Zero),
        [nameof(OllamaTextGenerationMetadata.Context)] = context
    };

    private static readonly IReadOnlyDictionary<string, object?> ReadOnlyTestDictionaryData = TestDictionaryData.AsReadOnly();


    [Fact]
    public void FromDictionaryWithCustomReadOnlyDictionaryShouldWorkCorrectly()
    {
        OllamaTextGenerationMetadata metadata = OllamaTextGenerationMetadata.FromDictionary(new MockIReadOnlyDictionary(TestDictionaryData));

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithDictionaryShouldWorkCorrectly()
    {
        OllamaTextGenerationMetadata metadata = OllamaTextGenerationMetadata.FromDictionary(TestDictionaryData);

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithReadOnlyDictionaryShouldWorkCorrectly()
    {
        OllamaTextGenerationMetadata metadata = OllamaTextGenerationMetadata.FromDictionary(ReadOnlyTestDictionaryData);

        AssertMetaData(metadata);
    }

    [Fact]
    public void FromDictionaryWithMetadataShouldWorkCorrectly()
    {
        OllamaTextGenerationMetadata metadataSource = OllamaTextGenerationMetadata.FromDictionary(ReadOnlyTestDictionaryData);

        OllamaTextGenerationMetadata metadata = OllamaTextGenerationMetadata.FromDictionary(metadataSource);

        AssertMetaData(metadata);
    }

    private static void AssertMetaData(OllamaTextGenerationMetadata metadata)
    {
        Assert.NotNull(metadata);

        Assert.Equal(16, metadata.PromptEvalCount);
        Assert.Equal(665900L, metadata.PromptEvalDuration);
        Assert.Equal(87, metadata.EvalCount);
        Assert.Equal("stop", metadata.DoneReason);
        Assert.Equal(5678600L, metadata.TotalDuration);
        Assert.Equal(17800L, metadata.LoadDuration);
        Assert.Equal(86700L, metadata.EvalDuration);
        Assert.Equal(true, metadata.Done);
        Assert.Equal(new DateTimeOffset(2024, 6, 5, 13, 38, 21, TimeSpan.Zero), metadata.CreatedAt);
        Assert.Equal(context, metadata.Context);
    }

    [Fact]
    public void FromDictionaryShouldThrowWithNullDictionary()
    {
        Assert.Throws<ArgumentNullException>(() => OllamaTextGenerationMetadata.FromDictionary(null));
    }
}