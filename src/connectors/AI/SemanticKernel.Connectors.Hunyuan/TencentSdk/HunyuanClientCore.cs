namespace IdeaTech.SemanticKernel.Connectors.Hunyuan;

/// <summary>
/// This class is responsible for making HTTP requests to the Hunyuan.
/// </summary>
internal sealed class HunyuanClientCore
{
    private const string ModelProvider = "Hunyuan";

    private readonly string _model;
    private readonly ILogger _logger;
    private readonly HunyuanClient _client;

    internal HunyuanClientCore(string model, Credential credential, string region, ClientProfile clientProfile, ILogger logger)
    {
        this._model = model;
        this._client = new HunyuanClient(credential, region, clientProfile);
        this._logger = logger;
    }

    internal async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        HunyuanPromptExecutionSettings hunyuanPromptExecutionSettings = HunyuanPromptExecutionSettings.FromExecutionSettings(executionSettings);
        hunyuanPromptExecutionSettings.ModelId ??= this._model;
        hunyuanPromptExecutionSettings.Stream = false;

        using var activity = ModelDiagnostics.StartCompletionActivity(new Uri(this._client.Endpoint), model, ModelProvider, chatHistory, hunyuanPromptExecutionSettings);

        ChatCompletionsRequest request = this.CreateChatRequest(chatHistory, hunyuanPromptExecutionSettings);

        ChatCompletionsResponse response;

        try
        {
            response = await this._client.ChatCompletions(request).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        List<ChatMessageContent> chatContents = GetChatMessageContentFromResponse(response, model);

        activity?.SetCompletionResponse(chatContents, (int?)response.Usage?.PromptTokens, (int?)response.Usage?.CompletionTokens);

        this.LogChatCompletionUsage(hunyuanPromptExecutionSettings, response);

        return chatContents;
    }

    internal async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        HunyuanPromptExecutionSettings hunyuanPromptExecutionSettings = HunyuanPromptExecutionSettings.FromExecutionSettings(executionSettings);
        hunyuanPromptExecutionSettings.ModelId ??= this._model;
        hunyuanPromptExecutionSettings.Stream = true;

        using var activity = ModelDiagnostics.StartCompletionActivity(new Uri(this._client.Endpoint), model, ModelProvider, chatHistory, hunyuanPromptExecutionSettings);

        ChatCompletionsRequest request = this.CreateChatRequest(chatHistory, hunyuanPromptExecutionSettings);

        ChatCompletionsResponse response;

        try
        {
            response = await this._client.ChatCompletions(request).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        IEnumerator<StreamingChatMessageContent> responseEnumerator = ProcessChatResponseStream(response, model);

        List<StreamingChatMessageContent>? streamedContents = activity is not null ? [] : null;

        try
        {
            while (true)
            {
                try
                {
                    if (!responseEnumerator.MoveNext())
                    {
                        break;
                    }
                }
                catch (Exception ex) when (activity is not null)
                {
                    activity.SetError(ex);
                    throw;
                }
            }

            streamedContents?.Add(responseEnumerator.Current);

            yield return responseEnumerator.Current;
        }
        finally
        {
            activity?.Dispose();
            responseEnumerator.Dispose();
        }
    }

    internal async Task<IReadOnlyList<TextContent>> GetChatAsTextContentsAsync(
        string text,
        PromptExecutionSettings? executionSettings,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        HunyuanPromptExecutionSettings chatSettings = HunyuanPromptExecutionSettings.FromExecutionSettings(executionSettings);

        ChatHistory chatHistory = CreateNewChat(text, chatSettings);

        return (await this.GetChatMessageContentsAsync(chatHistory, chatSettings).ConfigureAwait(false))
            .Select(chatMessageContent => new TextContent(
                chatMessageContent.Content,
                chatMessageContent.ModelId,
                chatMessageContent.Content,
                Encoding.UTF8,
                chatMessageContent.Metadata))
            .ToList();
    }

    internal async IAsyncEnumerable<StreamingTextContent> GetChatAsTextStreamingContentsAsync(
        string prompt,
        PromptExecutionSettings? executionSettings,
        Kernel? kernel,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        HunyuanPromptExecutionSettings chatSettings = HunyuanPromptExecutionSettings.FromExecutionSettings(executionSettings);
        ChatHistory chat = CreateNewChat(prompt, chatSettings);

        await foreach (StreamingChatMessageContent message in this.GetStreamingChatMessageContentsAsync(chat, executionSettings).ConfigureAwait(false))
        {
            yield return new StreamingTextContent(message.Content, message.ChoiceIndex, message.ModelId, message, Encoding.UTF8, message.Metadata);
        }
    }

    internal async Task<IList<ReadOnlyMemory<float>>> GetEmbeddingsAsync(
        IList<string> data,
        Kernel? kernel,
        CancellationToken cancellationToken)
    {
        if (data.Count != 1)
        {
            throw new NotSupportedException("Currently this interface does not support multiple embeddings results per data item, use only one data item");
        }

        List<ReadOnlyMemory<float>> result = new(data.Count);

        GetEmbeddingRequest request = new()
        {
            Input = data.SingleOrDefault()
        };

        GetEmbeddingResponse response = await this._client.GetEmbedding(request).ConfigureAwait(false);

        EmbeddingData[] embeddingsData = response.Data;

        if (embeddingsData.Length != data.Count)
        {
            throw new KernelException($"Expected {data.Count} text embedding(s), but received {embeddingsData.Length}");
        }

        for (var i = 0; i < embeddingsData.Length; i++)
        {
            float?[] embeddings = embeddingsData[i].Embedding;

            //🤣 Embedding and its element may be null
            if (embeddings is not null && embeddings.Length > 0)
            {
                if (embeddings.Any(item => item is null))
                {
                    throw new KernelException("The result of the embedding contains null elements.");
                }
                else
                {
                    result.Add(new ReadOnlyMemory<float>(embeddings.Select(x => x!.Value).ToArray()));
                }
            }
            else
            {
                throw new KernelException("The result of the embedding is null or empty");
            }
        }

        return result;
    }

    private static List<ChatMessageContent> GetChatMessageContentFromResponse(ChatCompletionsResponse response, string modelId)
    {
        List<ChatMessageContent> chatMessageContents = [];

        foreach (var choice in response.Choices)
        {
            HunyuanChatCompletionMetadata metadata = new()
            {
                Id = response.Id,
                Created = response.Created,
                Usage = response.Usage,
                Note = response.Note,
                RequestId = response.RequestId,
                ErrorMsg = response.ErrorMsg,
                FinishReason = choice.FinishReason,
                Delta = choice.Delta
            };

            chatMessageContents.Add(new ChatMessageContent(
                role: new AuthorRole(choice.Message.Role ?? AuthorRole.Assistant.ToString()),
                content: choice.Message.Content,
                modelId: modelId,
                innerContent: response,
                encoding: Encoding.UTF8,
                metadata: metadata));
        }
        return chatMessageContents;
    }

    private ChatCompletionsRequest CreateChatRequest(ChatHistory chatHistory, HunyuanPromptExecutionSettings hunyuanFaceExecutionSettings)
    {
        if (this._logger.IsEnabled(LogLevel.Trace))
        {
            this._logger.LogTrace("ChatHistory: {ChatHistory}, Settings: {Settings}",
                JsonSerializer.Serialize(chatHistory),
                JsonSerializer.Serialize(hunyuanFaceExecutionSettings));
        }

        ChatCompletionsRequest request = FromChatHistoryAndExecutionSettings(chatHistory, hunyuanFaceExecutionSettings);

        return request;
    }

    private static ChatCompletionsRequest FromChatHistoryAndExecutionSettings(ChatHistory chatHistory, HunyuanPromptExecutionSettings executionSettings)
    {
        return new ChatCompletionsRequest
        {
            Messages = chatHistory.Select(message => new Message
            {
                Content = message.Content,
                Role = message.Role.ToString(),
            }).ToArray(),
            Temperature = executionSettings.Temperature,
            Model = executionSettings.ModelId,
            TopP = executionSettings.TopP,
            EnableEnhancement = executionSettings.EnableEnhancement,
            StreamModeration = executionSettings.StreamModeration,
            Stream = executionSettings.Stream
        };
    }

    private static IEnumerator<StreamingChatMessageContent> ProcessChatResponseStream(ChatCompletionsResponse response, string modelId)
    {
        IEnumerator<AbstractSSEModel.SSE> responseEnumerator = response.GetEnumerator();

        while (true)
        {
            if (!responseEnumerator.MoveNext())
            {
                break;
            }
            else
            {
                ChatCompletionsResponse? sseResponse = JsonSerializer.Deserialize<ChatCompletionsResponse>(responseEnumerator.Current.Data);
                if (sseResponse is not null)
                {
                    yield return GetStreamingChatMessageContentFromStreamResponse(sseResponse, modelId);
                }
            }

        }
    }

    private static StreamingChatMessageContent GetStreamingChatMessageContentFromStreamResponse(ChatCompletionsResponse response, string modelId)
    {
        Choice? choice = response.Choices?.FirstOrDefault();
        if (choice is not null)
        {
            HunyuanChatCompletionMetadata metadata = new()
            {
                Id = response.Id,
                Created = response.Created,
                Usage = response.Usage,
                Note = response.Note,
                RequestId = response.RequestId,
                ErrorMsg = response.ErrorMsg,
                FinishReason = choice.FinishReason,
                Delta = choice.Delta
            };

            var streamChat = new StreamingChatMessageContent(
                choice.Delta?.Role is not null ? new AuthorRole(choice.Delta.Role) : null,
                choice.Delta?.Content,
                response,
                0,
                modelId,
                Encoding.UTF8,
                metadata);

            return streamChat;
        }

        throw new KernelException("Unexpected response from model")
        {
            Data = { { "ResponseData", response } },
        };
    }

    private static ChatHistory CreateNewChat(string? text = null, HunyuanPromptExecutionSettings? executionSettings = null)
    {
        var chat = new ChatHistory();

        // If settings is not provided, create a new chat with the text as the system prompt
        AuthorRole textRole = AuthorRole.System;

        if (!string.IsNullOrWhiteSpace(executionSettings?.ChatSystemPrompt))
        {
            chat.AddSystemMessage(executionSettings!.ChatSystemPrompt!);
            textRole = AuthorRole.User;
        }

        if (!string.IsNullOrWhiteSpace(text))
        {
            chat.AddMessage(textRole, text!);
        }

        return chat;
    }

    #region Logging and Meter

    private void LogChatCompletionUsage(HunyuanPromptExecutionSettings executionSettings, ChatCompletionsResponse chatCompletionResponse)
    {
        if (chatCompletionResponse.Usage is null)
        {
            this._logger.LogDebug("Token usage information unavailable.");
            return;
        }

        if (this._logger.IsEnabled(LogLevel.Information))
        {
            this._logger.LogInformation(
                "Prompt tokens: {PromptTokens}. Completion tokens: {CompletionTokens}. Total tokens: {TotalTokens}. ModelId: {ModelId}.",
                chatCompletionResponse.Usage.PromptTokens,
                chatCompletionResponse.Usage.CompletionTokens,
                chatCompletionResponse.Usage.TotalTokens,
                executionSettings.ModelId);
        }

        promptTokensCounter.Add(chatCompletionResponse.Usage.PromptTokens ?? 0);
        completionTokensCounter.Add(chatCompletionResponse.Usage.CompletionTokens ?? 0);
        totalTokensCounter.Add(chatCompletionResponse.Usage.TotalTokens ?? 0);
    }


    private static readonly string @namespace = typeof(HunyuanChatCompletionService).Namespace!;

    /// <summary>
    /// Instance of <see cref="Meter"/> for metrics.
    /// </summary>
    private static readonly Meter meter = new(@namespace);

    /// <summary>
    /// Instance of <see cref="Counter{T}"/> to keep track of the number of prompt tokens used.
    /// </summary>
    private static readonly Counter<long> promptTokensCounter =
        meter.CreateCounter<long>(
            name: $"{@namespace}.tokens.prompt",
            unit: "{token}",
            description: "Number of prompt tokens used");

    /// <summary>
    /// Instance of <see cref="Counter{T}"/> to keep track of the number of completion tokens used.
    /// </summary>
    private static readonly Counter<long> completionTokensCounter =
        meter.CreateCounter<long>(
            name: $"{@namespace}.tokens.completion",
            unit: "{token}",
            description: "Number of completion tokens used");

    /// <summary>
    /// Instance of <see cref="Counter{T}"/> to keep track of the total number of tokens used.
    /// </summary>
    private static readonly Counter<long> totalTokensCounter =
        meter.CreateCounter<long>(
            name: $"{@namespace}.tokens.total",
            unit: "{token}",
            description: "Number of total tokens used");

    #endregion

}
