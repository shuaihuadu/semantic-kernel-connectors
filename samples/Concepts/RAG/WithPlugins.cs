using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Plugins.Memory;

namespace RAG;

public class WithPlugins(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RAGWithCustomPluginAsync()
    {
        var kernel = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(TestConfiguration.Ollama.ModelId, TestConfiguration.Ollama.Endpoint)
            .Build();

        kernel.ImportPluginFromType<CustomPlugin>();

        var result = await kernel.InvokePromptAsync("{{search 'budget by year'}} What is my budget for 2024?");

        Console.WriteLine(result);
    }

    [Fact]
    public async Task RAGWithTextMemoryPluginAsync()
    {
        var memory = new MemoryBuilder()
            .WithMemoryStore(new VolatileMemoryStore())
            .WithOllamaTextEmbeddingGeneration(TestConfiguration.Ollama.EmbeddingModelId, TestConfiguration.Ollama.Endpoint)
            .Build();

        var kernel = Kernel.CreateBuilder()
            .AddOllamaChatCompletion(TestConfiguration.Ollama.ModelId, TestConfiguration.Ollama.Endpoint)
            .Build();

        kernel.ImportPluginFromObject(new TextMemoryPlugin(memory));

        var result = await kernel.InvokePromptAsync("{{recall 'budget by year' collection='finances'}} What is my budget for 2024?");

        Console.WriteLine(result);
    }

    #region Custom Plugin

    private sealed class CustomPlugin
    {
        [KernelFunction]
        public Task<string> SearchAsync(string query)
        {
            // Here will be a call to vector DB, return example result for demo purposes
            return Task.FromResult("Year Budget 2020 100,000 2021 120,000 2022 150,000 2023 200,000 2024 364,000");
        }
    }

    #endregion
}
