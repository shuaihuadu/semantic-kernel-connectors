// Copyright (c) IdeaTech. All rights reserved.

namespace IdeaTech.SemanticKernel.Connectors.Ollama;

internal static class AuthorRoleExtensions
{
    /// <summary>
    /// Convert the <see cref="AuthorRole"/> to <see cref="ChatMessageRole"/>.
    /// </summary>
    /// <param name="role">The author role.</param>
    /// <returns>The chat message role.</returns>
    public static ChatMessageRole ToChatMessageRole(this AuthorRole role)
    {
        if (role.Label.Equals("user", StringComparison.OrdinalIgnoreCase))
        {
            return ChatMessageRole.User;
        }
        else if (role.Label.Equals("system", StringComparison.OrdinalIgnoreCase))
        {
            return ChatMessageRole.System;
        }
        else
        {
            return ChatMessageRole.Assistant;
        }
    }
}
