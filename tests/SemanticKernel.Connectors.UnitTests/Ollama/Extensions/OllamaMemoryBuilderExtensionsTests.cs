namespace SemanticKernel.Connectors.UnitTests.Ollama.Extensions;

public sealed class OllamaMemoryBuilderExtensionsTests
{
    private readonly Mock<IMemoryStore> _mockMemoryStore = new();

    [Fact]
    public void OllamaTextEmbeddingGenerationWorksWithUriCorrectly()
    {
        MemoryBuilder builder = new();

        ISemanticTextMemory memory = builder.WithOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeUri)
            .WithMemoryStore(_mockMemoryStore.Object)
            .Build();

        Assert.NotNull(memory);
    }

    [Fact]
    public void OllamaTextEmbeddingGenerationWorksWithUriStringCorrectly()
    {
        MemoryBuilder builder = new();

        ISemanticTextMemory memory = builder.WithOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeUriString)
            .WithMemoryStore(_mockMemoryStore.Object)
            .Build();

        Assert.NotNull(memory);
    }

    [Fact]
    public void OllamaTextEmbeddingGenerationWorksWithHttpClientCorrectly()
    {
        MemoryBuilder builder = new();

        ISemanticTextMemory memory = builder.WithOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClient)
            .WithMemoryStore(_mockMemoryStore.Object)
            .Build();

        Assert.NotNull(memory);
    }

    [Fact]
    public static void OllamaTextEmbeddingGenerationWorksWithHttpClientWithNullBaseAddressShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => new MemoryBuilder().WithOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
    }
}