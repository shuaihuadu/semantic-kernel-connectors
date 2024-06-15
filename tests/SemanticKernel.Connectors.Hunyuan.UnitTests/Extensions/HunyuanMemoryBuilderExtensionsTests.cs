namespace SemanticKernel.Connectors.Hunyuan.UnitTests.Extensions;

public sealed class HunyuanMemoryBuilderExtensionsTests
{
    private readonly Mock<IMemoryStore> _mockMemoryStore = new();

    [Fact]
    public void HunyuanTextEmbeddingGenerationWorksCorrectly()
    {
        MemoryBuilder builder = new();

        ISemanticTextMemory memory = builder.WithHunyuanTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken)
            .WithMemoryStore(_mockMemoryStore.Object)
            .Build();

        Assert.NotNull(memory);
    }
}