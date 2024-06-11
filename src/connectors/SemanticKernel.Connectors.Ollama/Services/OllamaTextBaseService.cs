namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama text generation base service.
/// </summary>
public abstract class OllamaTextBaseService : OllamaBaseService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextGenerationService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri endpoint including the port where Ollama server is hosted.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    protected OllamaTextBaseService(string model, Uri endpoint, ILoggerFactory? loggerFactory = null) : base(model, endpoint, loggerFactory) { }

    /*
    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextBaseService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="endpoint">The uri string endpoint including the port where Ollama server is hosted</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    protected OllamaTextBaseService(string model, string endpoint, ILoggerFactory? loggerFactory = null) : this(model, new Uri(endpoint), loggerFactory) { }
    */

    /// <summary>
    /// Initializes a new instance of the <see cref="OllamaTextBaseService"/> class.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="httpClient">HTTP client to be used for communication with the Ollama API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    protected OllamaTextBaseService(string model, HttpClient httpClient, ILoggerFactory? loggerFactory = null) : base(model, httpClient, loggerFactory) { }

    /// <summary>
    /// Create the generate completion options use specified execution settings.
    /// </summary>
    /// <param name="model">The model name.</param>
    /// <param name="prompt">The prompt to generate a response for.</param>
    /// <param name="ollamaPromptExecutionSettings">The Ollama execution settings.</param>
    /// <param name="images">The images.</param>
    /// <returns></returns>
    protected static GenerateCompletionOptions CreateGenerateCompletionOptions(string model, string prompt, OllamaPromptExecutionSettings ollamaPromptExecutionSettings, string[]? images = null)
    {
        return new GenerateCompletionOptions
        {
            Model = model,
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

    /// <summary>
    /// Get the <see cref="StreamingTextContent"/> from <paramref name="response"/>.
    /// </summary>
    /// <param name="response">The generation completion response.</param>
    /// <returns></returns>
    protected static StreamingTextContent GetStreamingTextContentFromResponse(GenerateCompletionResponse response) => new(
        text: response.Response,
        modelId: response.Model,
        innerContent: response,
        metadata: new OllamaTextGenerationMetadata(response));

    /// <summary>
    /// Get the <see cref="TextContent"/> from <paramref name="response"/>.
    /// </summary>
    /// <param name="response">The generation completion response.</param>
    /// <returns></returns>
    protected static TextContent GetTextContentFromResponse(GenerateCompletionResponse response) => new(
        text: response.Response,
        modelId: response.Model,
        response,
        Encoding.UTF8,
        new OllamaTextGenerationMetadata(response));
}
