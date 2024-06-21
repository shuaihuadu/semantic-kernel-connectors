// Copyright (c) IdeaTech. All rights reserved.

using Microsoft.SemanticKernel.TextGeneration;

namespace Ollama.TextGeneration;

public class Ollama_TextGenerationStreaming(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunAsync()
    {
        Console.WriteLine("======== Ollama - Text Generation ========");

        Kernel kernel = Kernel.CreateBuilder()
            .AddOllamaTextGeneration(
                model: TestConfiguration.Ollama.ModelId,
                endpoint: TestConfiguration.Ollama.Endpoint)
            .Build();

        var questionAnswerFunction = kernel.CreateFunctionFromPrompt("Question: {{$input}}; Answer:");

        var result = await kernel.InvokeAsync(questionAnswerFunction, new() { ["input"] = "What is New York?" });

        Console.WriteLine(result.GetValue<string>());
    }

    [Fact]
    public Task RunStreamAsync()
    {
        Console.WriteLine("======== Ollama - Text Generation - Streaming ========");

        OllamaTextGenerationService textGeneration = new(
            model: TestConfiguration.Ollama.ModelId,
            endpoint: TestConfiguration.Ollama.Endpoint);

        return this.TextGenerationStreamAsync(textGeneration);
    }

    private async Task TextGenerationStreamAsync(ITextGenerationService textGeneration)
    {
        OllamaPromptExecutionSettings executionSettings = new()
        {
            MaxTokens = 100,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            Temperature = 1,
            TopP = 0.5
        };

        string prompt = "Write one paragraph why AI is awesome";

        Console.WriteLine("Prompt: " + prompt);

        await foreach (StreamingTextContent? content in textGeneration.GetStreamingTextContentsAsync(prompt, executionSettings))
        {
            Console.Write(content);
        }

        Console.WriteLine();
    }
}
