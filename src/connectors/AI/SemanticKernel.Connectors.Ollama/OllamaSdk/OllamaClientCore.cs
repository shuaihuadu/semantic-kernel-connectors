// Copyright (c) IdeaTech. All rights reserved.

namespace IdeaTech.SemanticKernel.Connectors.Ollama;

internal sealed class OllamaClientCore
{
    private const string ModelProvider = "ollama";

    private readonly string _model;

    private readonly ILogger _logger;

    private readonly OllamaClient _client;

    private readonly Uri _endpoint;

    #region Client Core

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaClientCore"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    internal OllamaClientCore(string model, Uri endpoint, ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(endpoint);
        Verify.NotNullOrWhiteSpace(endpoint.AbsoluteUri);

        this._model = model;
        this._client = new OllamaClient(endpoint, loggerFactory);
        this._logger = loggerFactory?.CreateLogger(this.GetType()) ?? NullLogger.Instance;
        this._endpoint = endpoint;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaClientCore"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    internal OllamaClientCore(string model, string endpoint, ILoggerFactory? loggerFactory = null) : this(model, new Uri(endpoint), loggerFactory)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaClientCore"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="httpClient">HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    internal OllamaClientCore(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNull(httpClient);
        Verify.NotNull(httpClient.BaseAddress);
        Verify.NotNullOrWhiteSpace(httpClient.BaseAddress.AbsoluteUri);

        this._model = model;
        this._client = new OllamaClient(httpClient, loggerFactory);
        this._logger = loggerFactory?.CreateLogger(this.GetType()) ?? NullLogger.Instance;
        this._endpoint = httpClient.BaseAddress;
    }

    #endregion

    #region Text Generation

    internal async IAsyncEnumerable<StreamingTextContent> StreamGenerateTextAsync(string prompt, PromptExecutionSettings? executionSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, prompt, ollamaPromptExecutionSettings);

        StreamingResponse<GenerateCompletionResponse> response;

        try
        {
            response = await this._client.GenerateCompletionStreamingAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        ConfiguredCancelableAsyncEnumerable<GenerateCompletionResponse>.Enumerator responseEnumerator = response.ConfigureAwait(false).GetAsyncEnumerator();

        List<StreamingTextContent>? streamedContents = activity is not null ? [] : null;

        try
        {
            while (true)
            {
                try
                {
                    if (!await responseEnumerator.MoveNextAsync())
                    {
                        break;
                    }
                }
                catch (Exception ex) when (activity is not null)
                {
                    activity.SetError(ex);
                    throw;
                }

                GenerateCompletionResponse currentResponse = responseEnumerator.Current;

                StreamingTextContent content = GetStreamingTextContentFromResponse(currentResponse);

                streamedContents?.Add(content);

                yield return content;
            }
        }
        finally
        {
            activity?.EndStreaming(streamedContents);
            await responseEnumerator.DisposeAsync();
        }
    }

    internal async Task<IReadOnlyList<TextContent>> GenerateTextAsync(string prompt, PromptExecutionSettings? executionSettings = null, CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, prompt, ollamaPromptExecutionSettings);

        GenerateCompletionResponse response;

        try
        {
            response = await this._client.GenerateCompletionAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        TextContent content = GetTextContentFromResponse(response);

        activity?.SetCompletionResponse([content]);

        return [content];
    }

    #endregion

    #region Chat Completion

    internal async IAsyncEnumerable<StreamingChatMessageContent> StreamCompleteChatMessageAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, chatHistory, ollamaPromptExecutionSettings);

        StreamingResponse<ChatCompletionResponse> response;

        try
        {
            response = await this._client.ChatCompletionStreamingAsync(CreateChatCompletionOptions(model, chatHistory, ollamaPromptExecutionSettings), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        ConfiguredCancelableAsyncEnumerable<ChatCompletionResponse>.Enumerator responseEnumerator = response.ConfigureAwait(false).GetAsyncEnumerator();

        List<StreamingChatMessageContent>? streamedContents = activity is not null ? [] : null;

        try
        {
            while (true)
            {
                try
                {
                    if (!await responseEnumerator.MoveNextAsync())
                    {
                        break;
                    }
                }
                catch (Exception ex) when (activity is not null)
                {
                    activity.SetError(ex);
                    throw;
                }

                ChatCompletionResponse currentResponse = responseEnumerator.Current;

                StreamingChatMessageContent content = GetStreamingChatMessageContentFromResponse(currentResponse);

                streamedContents?.Add(content);

                yield return content;
            }
        }
        finally
        {
            activity?.EndStreaming(streamedContents);
            await responseEnumerator.DisposeAsync();
        }
    }

    internal async Task<IReadOnlyList<ChatMessageContent>> CompleteChatMessageAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, CancellationToken cancellationToken = default)
    {
        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, chatHistory, ollamaPromptExecutionSettings);

        ChatCompletionResponse response;

        try
        {
            response = await this._client.ChatCompletionAsync(CreateChatCompletionOptions(model, chatHistory, ollamaPromptExecutionSettings), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        ChatMessageContent content = GetChatMessageContentFromResponse(response);

        activity?.SetCompletionResponse([content], response.PromptEvalCount, response.EvalCount);

        this.LogChatCompletionUsage(ollamaPromptExecutionSettings, response);

        return [content];
    }

    #endregion

    #region Embeddings

    internal async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken)
    {
        Verify.NotNullOrEmpty(data);

        if (data.Count != 1)
        {
            throw new NotSupportedException("Currently this interface does not support multiple embeddings results per data item, use only one data item");
        }

        EmbeddingResponse response = await this._client.GenerateEmbeddingAsync(this._model, data[0], cancellationToken);

        return [response.Embedding];
    }

    #endregion

    #region Image To Text

    internal async Task<IReadOnlyList<TextContent>> GenerateTextFromImageAsync(ImageContent content, PromptExecutionSettings? executionSettings, CancellationToken cancellationToken)
    {
        Verify.NotNull(content);
        Verify.NotNull(content.Data);
        Verify.NotNullOrEmpty(content.Data.Value.ToArray());

        string prompt = "What is in this image?";

        string model = executionSettings?.ModelId ?? this._model;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        string imageBase64Data = Convert.ToBase64String(content.Data.Value.ToArray());

        using Activity? activity = ModelDiagnostics.StartCompletionActivity(this._endpoint, model, ModelProvider, prompt, executionSettings);

        GenerateCompletionResponse response;

        try
        {
            response = await this._client.GenerateCompletionAsync(CreateGenerateCompletionOptions(model, prompt, ollamaPromptExecutionSettings, [imageBase64Data]), cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (activity is not null)
        {
            activity.SetError(ex);
            throw;
        }

        TextContent textContent = GetTextContentFromResponse(response);

        activity?.SetCompletionResponse([textContent]);

        return [textContent];
    }

    #endregion

    #region Logging and Meter

    private void LogChatCompletionUsage(OllamaPromptExecutionSettings executionSettings, ChatCompletionResponse chatCompletionResponse)
    {
        if (this._logger.IsEnabled(LogLevel.Information))
        {
            this._logger.LogInformation(
                "Prompt tokens: {PromptTokens}. Completion tokens: {CompletionTokens}. Total tokens: {TotalTokens}. ModelId: {ModelId}.",
                chatCompletionResponse.PromptEvalCount,
                chatCompletionResponse.EvalCount,
                chatCompletionResponse.PromptEvalCount + chatCompletionResponse.EvalCount,
                executionSettings.ModelId);
        }

        promptTokensCounter.Add(chatCompletionResponse.PromptEvalCount);
        completionTokensCounter.Add(chatCompletionResponse.EvalCount);
        totalTokensCounter.Add(chatCompletionResponse.PromptEvalCount + chatCompletionResponse.EvalCount);
    }

    private static readonly string @namespace = typeof(OllamaChatCompletionService).Namespace!;

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

    #region Private

    private static GenerateCompletionOptions CreateGenerateCompletionOptions(string model, string prompt, OllamaPromptExecutionSettings ollamaPromptExecutionSettings, string[]? images = null)
    {
        return new GenerateCompletionOptions
        {
            Model = ollamaPromptExecutionSettings.ModelId ?? model,
            Prompt = prompt,
            Format = ollamaPromptExecutionSettings.Format,
            KeepAlive = ollamaPromptExecutionSettings.KeepAlive,
            System = ollamaPromptExecutionSettings.SystemPrompt,
            Images = images,
            Options = new ParameterOptions
            {
                NumCtx = ollamaPromptExecutionSettings.MaxTokens,
                FrequencyPenalty = ollamaPromptExecutionSettings.FrequencyPenalty,
                PresencePenalty = ollamaPromptExecutionSettings.PresencePenalty,
                Temperature = ollamaPromptExecutionSettings.Temperature,
                Seed = (int)ollamaPromptExecutionSettings.Seed,
                Stop = ollamaPromptExecutionSettings.Stop?.ToArray(),
                TopK = ollamaPromptExecutionSettings.TopK,
                TopP = ollamaPromptExecutionSettings.TopP
            }
        };
    }

    private static StreamingTextContent GetStreamingTextContentFromResponse(GenerateCompletionResponse response) => new(
        text: response.Response,
        modelId: response.Model,
        innerContent: response,
        metadata: new OllamaTextGenerationMetadata(response));

    private static TextContent GetTextContentFromResponse(GenerateCompletionResponse response) => new(
        text: response.Response,
        modelId: response.Model,
        response,
        Encoding.UTF8,
        new OllamaTextGenerationMetadata(response));

    private static ChatCompletionOptions CreateChatCompletionOptions(string model, ChatHistory chatHistory, OllamaPromptExecutionSettings ollamaPromptExecutionSettings)
    {
        return new ChatCompletionOptions
        {
            Model = ollamaPromptExecutionSettings.ModelId ?? model,
            Messages = [.. chatHistory.Select(message => message.ToChatMessage())],
            Format = ollamaPromptExecutionSettings.Format,
            KeepAlive = ollamaPromptExecutionSettings.KeepAlive,
            Options = new ParameterOptions
            {
                NumCtx = ollamaPromptExecutionSettings.MaxTokens,
                FrequencyPenalty = ollamaPromptExecutionSettings.FrequencyPenalty,
                PresencePenalty = ollamaPromptExecutionSettings.PresencePenalty,
                Temperature = ollamaPromptExecutionSettings.Temperature,
                Seed = (int)ollamaPromptExecutionSettings.Seed,
                Stop = ollamaPromptExecutionSettings.Stop?.ToArray(),
                TopK = ollamaPromptExecutionSettings.TopK,
                TopP = ollamaPromptExecutionSettings.TopP
            }
        };
    }

    private static StreamingChatMessageContent GetStreamingChatMessageContentFromResponse(ChatCompletionResponse response) => new(
        role: response.Message?.Role is not null ? new AuthorRole(response.Message.Role.Value.Label) : null,
        content: response.Message?.Content,
        innerContent: response,
        modelId: response.Model,
        encoding: Encoding.UTF8,
        metadata: new OllamaChatGenerationMetadata(response));

    private static ChatMessageContent GetChatMessageContentFromResponse(ChatCompletionResponse response) => new(
        role: response.Message?.Role is not null ? new AuthorRole(response.Message.Role.Value.Label) : AuthorRole.Assistant,
        content: response.Message?.Content,
        modelId: response.Model,
        innerContent: response,
        encoding: Encoding.UTF8,
        metadata: new OllamaChatGenerationMetadata(response));

    #endregion
}
