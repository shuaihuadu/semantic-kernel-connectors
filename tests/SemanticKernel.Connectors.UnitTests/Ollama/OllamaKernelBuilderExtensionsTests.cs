using Microsoft.SemanticKernel.Embeddings;

namespace SemanticKernel.Connectors.UnitTests.Ollama;

public class OllamaKernelBuilderExtensionsTests
{

    [Fact]
    public void AddOllamaTextGenerationWithUriStringCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextGeneration("model", "http://localhost");

        Kernel kernel = builder.Build();

        ITextGenerationService service = kernel.GetRequiredService<ITextGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextGenerationWithUriCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextGeneration("model", new Uri("http://localhost"));

        Kernel kernel = builder.Build();

        ITextGenerationService service = kernel.GetRequiredService<ITextGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaChatCompletionWithUriStringCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion("model", "http://localhost");

        Kernel kernel = builder.Build();

        IChatCompletionService service = kernel.GetRequiredService<IChatCompletionService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }

    [Fact]
    public void AddOllamaChatCompletionWithUriCreatesService2()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion("model", new Uri("http://localhost"));

        Kernel kernel = builder.Build();

        IChatCompletionService service = kernel.GetRequiredService<IChatCompletionService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithUriStringCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextEmbeddingGeneration("model", "http://localhost");

        Kernel kernel = builder.Build();

        ITextEmbeddingGenerationService service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithUriCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextEmbeddingGeneration("model", new Uri("http://localhost"));

        Kernel kernel = builder.Build();

        ITextEmbeddingGenerationService service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }
}
