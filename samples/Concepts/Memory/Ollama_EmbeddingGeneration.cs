using Microsoft.SemanticKernel.Embeddings;

namespace Memory;

public class Ollama_EmbeddingGeneration(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunEmbeddingAsync()
    {
        Console.WriteLine("\n======= Ollama Embedding Example ========\n");

        Kernel kernel = Kernel.CreateBuilder()
            .AddOllamaTextEmbeddingGeneration(model: TestConfiguration.Ollama.EmbeddingModelId, endpoint: TestConfiguration.Ollama.Endpoint)
            .Build();

        var embeddingGenerator = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        // Generate embeddings for each chunk.
        var embeddings = await embeddingGenerator.GenerateEmbeddingsAsync(["John: Hello, how are you?\nRoger: Hey, I'm Roger!"]);

        Console.WriteLine($"Generated {embeddings.Count} embeddings for the provided text");
        Console.WriteLine($"The embedding array length {embeddings[0].Length}");
    }
}
