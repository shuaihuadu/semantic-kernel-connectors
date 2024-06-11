namespace ChatCompletion;

public class Ollama_ChatCompletion(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunAsync()
    {
        await OllamaChatSampleAsync();
    }

    private async Task OllamaChatSampleAsync()
    {
        Console.WriteLine("======== Ollama - Chat ========");

        OllamaChatCompletionService chatCompletionService = new(TestConfiguration.Ollama.ModelId, TestConfiguration.Ollama.Endpoint);

        await StartChatAsync(chatCompletionService);
    }

    private async Task StartChatAsync(IChatCompletionService chatCompletionService)
    {
        Console.WriteLine("Chat content:");
        Console.WriteLine("------------------------");

        var chatHistory = new ChatHistory("You are a librarian, expert about books");

        chatHistory.AddUserMessage("Hi, I'm looking for book suggestions");
        await MessageOutputAsync(chatHistory);

        ChatMessageContent reply = await chatCompletionService.GetChatMessageContentAsync(chatHistory, new OllamaPromptExecutionSettings
        {
            MaxTokens = 1
        });
        chatHistory.Add(reply);
        await MessageOutputAsync(chatHistory);

        chatHistory.AddUserMessage("I love history and philosophy, I'd like to learn something new about Greece, any suggestion");
        await MessageOutputAsync(chatHistory);

        reply = await chatCompletionService.GetChatMessageContentAsync(chatHistory, new OllamaPromptExecutionSettings
        {
            MaxTokens = 1
        });
        chatHistory.Add(reply);
        await MessageOutputAsync(chatHistory);
    }

    private Task MessageOutputAsync(ChatHistory chatHistory)
    {
        ChatMessageContent message = chatHistory.Last();

        Console.WriteLine($"{message.Role}: {message.Content}");
        Console.WriteLine("------------------------");

        return Task.CompletedTask;
    }
}
