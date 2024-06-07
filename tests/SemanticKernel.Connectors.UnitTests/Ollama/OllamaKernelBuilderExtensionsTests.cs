namespace SemanticKernel.Connectors.UnitTests.Ollama;

public class OllamaKernelBuilderExtensionsTests
{

    [Fact]
    public void AddOllamaTextGenerationCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextGeneration("model", "http://localhost");

        Kernel kernel = builder.Build();

        ITextGenerationService service = kernel.GetRequiredService<ITextGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }
}
