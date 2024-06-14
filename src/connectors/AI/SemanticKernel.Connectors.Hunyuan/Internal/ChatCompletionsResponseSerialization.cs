namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

internal class ChatCompletionsResponseSerialization
{
    internal static ChatCompletionsResponse? DeserializeChatCompletionsResponse(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        string? id = default;
        string? note = default;
        long created = default;
        Usage? usage = default;
        Choice[] choices = [];
        ErrorMsg? errorMsg = default;

        foreach (var property in element.EnumerateObject())
        {
            if (property.NameEquals("Id"u8))
            {
                id = property.Value.GetString();
                continue;
            }

            if (property.NameEquals("Choices"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                List<Choice> array = [];

                foreach (var item in property.Value.EnumerateArray())
                {
                    Choice? choice = DeserializeChatCompletionsResponseChoice(item);

                    if (choice is not null)
                    {
                        array.Add(choice);
                    }
                }
                choices = [.. array];
                continue;
            }

            if (property.NameEquals("Note"u8))
            {
                note = property.Value.GetString();
                continue;
            }

            if (property.NameEquals("Created"u8))
            {
                created = property.Value.GetInt64();
                continue;
            }

            if (property.NameEquals("Usage"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }
                usage = DeserializeChatCompletionsResponseUsage(property.Value);
                continue;
            }

            if (property.NameEquals("ErrorMsg"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }
                errorMsg = DeserializeChatCompletionsResponseErrorMsg(property.Value);
                continue;
            }
        }

        return new ChatCompletionsResponse
        {
            Id = id,
            Note = note,
            Created = created,
            Usage = usage,
            Choices = choices,
            ErrorMsg = errorMsg,
            RequestId = id
        };
    }

    internal static Choice? DeserializeChatCompletionsResponseChoice(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        Delta? delta = default;
        string? finishReason = string.Empty;

        foreach (JsonProperty property in element.EnumerateObject())
        {
            if (property.NameEquals("Delta"u8))
            {
                delta = DeserializeChatCompletionResponseChoiceDelta(property.Value);

                continue;
            }

            if (property.NameEquals("FinishReason"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    finishReason = null;
                    continue;
                }

                finishReason = property.Value.GetString();
                continue;
            }
        }

        return new Choice { Delta = delta, FinishReason = finishReason };
    }

    internal static Delta? DeserializeChatCompletionResponseChoiceDelta(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        string? content = default, role = default;

        foreach (var property in element.EnumerateObject())
        {
            if (property.NameEquals("Content"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                content = property.Value.GetString();
                continue;
            }
            if (property.NameEquals("Role"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }
                role = property.Value.GetString();
                continue;
            }
        }

        return new Delta { Content = content ?? string.Empty, Role = role ?? string.Empty };
    }

    internal static Usage? DeserializeChatCompletionsResponseUsage(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        int promptTokens = default, completionTokens = default, totalTokens = default;

        foreach (var property in element.EnumerateObject())
        {
            if (property.NameEquals("PromptTokens"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                promptTokens = property.Value.GetInt32();
                continue;
            }

            if (property.NameEquals("CompletionTokens"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                completionTokens = property.Value.GetInt32();
                continue;
            }

            if (property.NameEquals("TotalTokens"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                totalTokens = property.Value.GetInt32();
                continue;
            }
        }

        return new Usage { PromptTokens = promptTokens, CompletionTokens = completionTokens, TotalTokens = totalTokens };
    }

    internal static ErrorMsg? DeserializeChatCompletionsResponseErrorMsg(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        long code = default;
        string? msg = default;

        foreach (var property in element.EnumerateObject())
        {
            if (property.NameEquals("Code"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }

                code = property.Value.GetInt64();
                continue;
            }
            if (property.NameEquals("Msg"u8))
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    continue;
                }
                msg = property.Value.GetString();
                continue;
            }
        }

        return new ErrorMsg { Code = code, Msg = msg ?? string.Empty };
    }
}
