// Copyright (c) IdeaTech. All rights reserved.

namespace IdeaTech.SemanticKernel.Connectors.Ollama;

/// <summary>
/// Ollama Execution Settings.
/// </summary>
public sealed class OllamaPromptExecutionSettings : PromptExecutionSettings
{
    /// <summary>
    /// Gets the specialization for the Ollama execution settings.
    /// </summary>
    /// <param name="executionSettings">Generic prompt execution settings.</param>
    /// <returns>Specialized Ollama execution settings.</returns>
    public static OllamaPromptExecutionSettings FromExecutionSettings(PromptExecutionSettings? executionSettings)
    {
        switch (executionSettings)
        {
            case null:
                return new OllamaPromptExecutionSettings();
            case OllamaPromptExecutionSettings settings:
                return settings;
        }

        var json = JsonSerializer.Serialize(executionSettings);

        OllamaPromptExecutionSettings? ollamaPromptExecutionSettings = JsonSerializer.Deserialize<OllamaPromptExecutionSettings>(json, JsonOptionsCache.ReadPermissive);

        return ollamaPromptExecutionSettings!;
    }

    /// <summary>
    /// The temperature of the model.Increasing the temperature will make the model answer more creatively. (Default: 0.8).
    /// </summary>
    [JsonPropertyName("temperature")]
    public double Temperature
    {
        get => this._temperature;

        set
        {
            this.ThrowIfFrozen();
            this._temperature = value;
        }
    }

    /// <summary>
    /// Reduces the probability of generating nonsense.
    /// A higher value(e.g. 100) will give more diverse answers, while a lower value(e.g. 10) will be more conservative. (Default: 40).
    /// </summary>
    [JsonPropertyName("top_k")]
    public int TopK
    {
        get => this._topK;

        set
        {
            this.ThrowIfFrozen();
            this._topK = value;
        }
    }

    /// <summary>
    /// Sets the size of the context window used to generate the next token. (Default: 2048).
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int MaxTokens
    {
        get => this._maxTokens;

        set
        {
            this.ThrowIfFrozen();
            this._maxTokens = value;
        }
    }

    /// <summary>
    /// Controls how long the model will stay loaded into memory following the request (default: 5m).
    /// <para>The keep_alive parameter can be set to:</para>
    /// <list type="bullet"> a duration string (such as "10m" or "24h")
    /// <item> a number in seconds(such as 3600) </item>
    /// <item> any negative number which will keep the model loaded in memory(e.g. -1 or "-1m") </item>
    /// <item> '0' which will unload the model immediately after generating a response </item>
    /// </list>
    /// </summary>
    [JsonPropertyName("keep_alive")]
    public double KeepAlive
    {
        get => this._keepAlive;

        set
        {
            this.ThrowIfFrozen();
            this._keepAlive = value;
        }
    }

    /// <summary>
    /// Works together with top-k.
    /// A higher value(e.g., 0.95) will lead to more diverse text, while a lower value(e.g., 0.5) will generate more focused and conservative text. (Default: 0.9).
    /// </summary>
    [JsonPropertyName("top_p")]
    public double TopP
    {
        get => this._topP;

        set
        {
            this.ThrowIfFrozen();
            this._topP = value;
        }
    }

    /// <summary>
    /// Presence penalty coefficient. Used to reduce the generation of repeated words; higher values penalize words that have already appeared.
    /// </summary>
    [JsonPropertyName("presence_penalty")]
    public double PresencePenalty
    {
        get => this._presencePenalty;

        set
        {
            this.ThrowIfFrozen();
            this._presencePenalty = value;
        }
    }

    /// <summary>
    /// Frequency penalty coefficient. Used to reduce the generation of high-frequency words; higher values penalize frequently occurring words.
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public double FrequencyPenalty
    {
        get => this._frequencyPenalty;

        set
        {
            this.ThrowIfFrozen();
            this._frequencyPenalty = value;
        }
    }

    /// <summary>
    /// Sets the random number seed to use for generation. <br />
    /// Setting this to a specific number will make the model generate the same text for the same prompt. (Default: 0). <br />
    /// For reproducible outputs, set temperature to 0 and seed to a number.
    /// </summary>
    [JsonPropertyName("seed")]
    public long Seed
    {
        get => this._seed;

        set
        {
            this.ThrowIfFrozen();
            this._seed = value;
        }
    }

    /// <summary>
    /// Sets the stop sequences to use.When this pattern is encountered the LLM will stop generating text and return.
    /// Multiple stop patterns may be set by specifying multiple separate stop parameters in a modelfile.
    /// </summary>
    [JsonPropertyName("stop")]
    public List<string>? Stop
    {
        get => this._stop;

        set
        {
            this.ThrowIfFrozen();
            this._stop = value;
        }
    }

    /// <summary>
    /// The format to return a response in.
    /// Currently the only accepted value is json.
    /// When format is set to json, the output will always be a well-formed JSON object. It's important to also instruct the model to respond in JSON.
    /// </summary>
    [JsonPropertyName("format")]
    public string? Format
    {
        get => this._format;

        set
        {
            this.ThrowIfFrozen();
            this._format = value;
        }
    }

    /// <summary>
    /// The system prompt to use when generating text.
    /// Defaults to "Assistant is a large language model."
    /// </summary>
    [JsonPropertyName("system_prompt")]
    public string? SystemPrompt
    {
        get => this._systemPrompt;

        set
        {
            this.ThrowIfFrozen();
            this._systemPrompt = value;
        }
    }

    /// <inheritdoc />
    public override PromptExecutionSettings Clone()
    {
        return new OllamaPromptExecutionSettings()
        {
            ModelId = this.ModelId,
            Temperature = this.Temperature,
            MaxTokens = this.MaxTokens,
            TopP = this.TopP,
            TopK = this.TopK,
            FrequencyPenalty = this._frequencyPenalty,
            PresencePenalty = this.PresencePenalty,
            Format = this.Format,
            KeepAlive = this.KeepAlive,
            Seed = this.Seed,
            Stop = this.Stop is not null ? new List<string>(this.Stop) : null,
            ExtensionData = this.ExtensionData is not null ? new Dictionary<string, object>(this.ExtensionData) : null
        };
    }

    private double _frequencyPenalty = 1.1;

    private double _presencePenalty = 0.8;

    private long _seed = 0;

    private List<string>? _stop;

    private double _temperature = 0.8;

    private double _topP = 0.9;

    private int _maxTokens = 2048;

    private double _keepAlive = 3000;

    private int _topK = 40;

    private string? _format;

    private string? _systemPrompt = "Assistant is a large language model.";
}
