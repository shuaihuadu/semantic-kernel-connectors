using IdeaTech.SemanticKernel.Connectors.Ollama;

namespace SemanticKernel.Connectors.Ollama.UnitTests;

public class OllamaTextGenerationTests
{
    [Fact]
    public async Task SpecifiedModelShouldBeUsedAsync()
    {
        OllamaTextGenerationService ollamaTextGenerationService = new("llama3", "http://localhost:11434");

        var a = await ollamaTextGenerationService.GetTextContentsAsync("fake-text");
    }
}