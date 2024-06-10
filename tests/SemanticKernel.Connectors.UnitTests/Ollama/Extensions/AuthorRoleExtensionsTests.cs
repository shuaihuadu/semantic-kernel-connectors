namespace SemanticKernel.Connectors.UnitTests.Ollama.Extensions;

public static class AuthorRoleExtensionsTests
{
    [Fact]
    public static void AuthorRoleShouldConvertToChatMessageRoleCorrectly()
    {
        //User
        AuthorRole user = AuthorRole.User;
        ChatMessageRole userRole = user.ToChatMessageRole();
        Assert.True(userRole == ChatMessageRole.User);

        //System
        AuthorRole system = AuthorRole.System;
        ChatMessageRole systemRole = system.ToChatMessageRole();
        Assert.True(systemRole == ChatMessageRole.System);

        //Assistant
        AuthorRole assistant = AuthorRole.Assistant;
        ChatMessageRole assistantRole = assistant.ToChatMessageRole();
        Assert.True(assistantRole == ChatMessageRole.Assistant);

        AuthorRole tool = AuthorRole.Tool;
        ChatMessageRole toolRole = tool.ToChatMessageRole();
        Assert.True(toolRole == ChatMessageRole.Assistant);
    }
}