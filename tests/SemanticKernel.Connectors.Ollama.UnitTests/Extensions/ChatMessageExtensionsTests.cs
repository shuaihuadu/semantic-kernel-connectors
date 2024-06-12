namespace SemanticKernel.Connectors.Ollama.UnitTests.Extensions;

public class ChatMessageExtensionsTests
{
    [Fact]
    public void ChatMessageContentShouldConvertCorrectly()
    {
        ChatMessageContent message = new(AuthorRole.User, "Test");
        message.Items.Add(new ImageContent(new ReadOnlyMemory<byte>([1, 2, 3])));
        message.Items.Add(new ImageContent(new ReadOnlyMemory<byte>([4, 5, 6])));

        ChatMessage ollamaChatMessage = message.ToChatMessage();

        Assert.Equal(ChatMessageRole.User, ollamaChatMessage.Role);
        Assert.Same(message.Content, ollamaChatMessage.Content);
        Assert.NotNull(ollamaChatMessage.Images);
        Assert.NotEmpty(ollamaChatMessage.Images);
        Assert.Equal(2, ollamaChatMessage.Images.Length);
    }

    [Fact]
    public void ChatMessageContentWithNullContentShouldConvertCorrectly()
    {
        ChatMessageContent message = new()
        {
            Role = AuthorRole.Assistant
        };

        ChatMessage ollamaChatMessage = message.ToChatMessage();

        Assert.Equal(ChatMessageRole.Assistant, ollamaChatMessage.Role);
        Assert.Empty(ollamaChatMessage.Content);
    }

    [Fact]
    public void ChatMessageRoleWithNullMessageShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => default(ChatMessageContent).ToChatMessage());
    }
}