namespace IdeaTech.SemanticKernel.Connectors.Ollama;

internal static class ChatMessageExtensions
{
    /// <summary>
    /// Convert the <see cref="ChatMessageContent"/> to <see cref="ChatMessage"/>.
    /// </summary>
    /// <param name="message">The sk chat message content.</param>
    /// <returns>The chat message role.</returns>
    public static ChatMessage ToChatMessageRole(this ChatMessageContent message)
    {
        return new ChatMessage
        {
            Content = message.Content ?? string.Empty,
            Role = message.Role.ToChatMessageRole(),
            Images = message.GetChatMessageContentImages()
        };
    }

    /// <summary>
    /// Get the image base64 content from <see cref="ChatMessageContent"/>.
    /// </summary>
    /// <param name="message">The sk chat message content.</param>
    /// <returns></returns>
    public static string[] GetChatMessageContentImages(this ChatMessageContent message)
    {
        List<string> result = [];

        foreach (var item in message.Items)
        {
            if (item is ImageContent)
            {
                var imageContent = item as ImageContent;

                if (imageContent is not null && imageContent.Data is not null)
                {
                    result.Add(Convert.ToBase64String(imageContent.Data.Value.Span.ToArray()));
                }
            }
        }

        return [.. result];
    }
}
