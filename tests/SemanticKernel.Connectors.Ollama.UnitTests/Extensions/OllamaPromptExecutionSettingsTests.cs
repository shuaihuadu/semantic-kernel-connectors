namespace SemanticKernel.Connectors.Ollama.UnitTests.Extensions;

public class OllamaPromptExecutionSettingsTests
{
    [Fact]
    public void FromExecutionSettingsWhenNewShouldReturnSame()
    {
        OllamaPromptExecutionSettings executionSettings = new();

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        Assert.Same(executionSettings, ollamaPromptExecutionSettings);
    }

    [Fact]
    public void FromExecutionSettingsWhenNullShouldReturnDefault()
    {
        OllamaPromptExecutionSettings? executionSettings = null;

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        Assert.NotNull(ollamaPromptExecutionSettings);
    }


    [Fact]
    public void FromExecutionSettingsWhenDeserializeShouldReturnEqualValues()
    {
        PromptExecutionSettings? executionSettings = new()
        {
            ModelId = TestConstants.FakeModel,
            ExtensionData = new Dictionary<string, object>
            {
                ["key"] = "The value"
            }
        };

        OllamaPromptExecutionSettings ollamaPromptExecutionSettings = OllamaPromptExecutionSettings.FromExecutionSettings(executionSettings);

        Assert.NotNull(ollamaPromptExecutionSettings);
        Assert.Equal(executionSettings.ModelId, ollamaPromptExecutionSettings.ModelId);
        Assert.Equal(executionSettings.ExtensionData["key"].ToString(), ollamaPromptExecutionSettings!.ExtensionData!["key"].ToString());
    }

    [Fact]
    public void PromptExecutionSettingsCloneWorksAsExpected()
    {
        string configPayload = """
        {
            "max_tokens": 100,
            "temperature": 0.5,
            "seed": 100,
            "stop": [
                "stop_sequence"
            ],
            "top_k": 100,
            "top_p": 0.2,
            "presence_penalty": 1.4,
            "frequency_penalty": 1.2,
            "keep_alive": 500,
            "system_prompt": "You are an AI Assistant",
            "format":"json"
        }
        """;
        var executionSettings = JsonSerializer.Deserialize<OllamaPromptExecutionSettings>(configPayload);
        executionSettings!.ExtensionData = new Dictionary<string, object>
        {
            ["key1"] = 1
        };

        var clone = executionSettings.Clone() as OllamaPromptExecutionSettings;

        Assert.NotNull(clone);
        Assert.Equal(executionSettings.ModelId, clone.ModelId);
        Assert.Equivalent(executionSettings.ExtensionData, clone.ExtensionData);
        Assert.NotNull(clone.ExtensionData);
        Assert.NotEmpty(clone.ExtensionData);
        Assert.Equal(1, clone.ExtensionData["key1"]);
        Assert.Equal(executionSettings.Temperature, clone.Temperature);
        Assert.Equal(executionSettings.TopK, clone.TopK);
        Assert.Equal(executionSettings.KeepAlive, clone.KeepAlive);
        Assert.Equal(executionSettings.TopP, clone.TopP);
        Assert.Equal(executionSettings.PresencePenalty, clone.PresencePenalty);
        Assert.Equal(executionSettings.FrequencyPenalty, clone.FrequencyPenalty);
        Assert.Equal(executionSettings.Seed, clone.Seed);
        Assert.Equivalent(executionSettings.Stop, clone.Stop);
        Assert.Equal(executionSettings.Format, clone.Format);
    }
    [Fact]
    public void PromptExecutionSettingsWithNullValuesCloneWorksAsExpected()
    {
        string configPayload = """
        {
            "stop": null
        }
        """;
        var executionSettings = JsonSerializer.Deserialize<OllamaPromptExecutionSettings>(configPayload);

        var clone2 = executionSettings!.Clone() as OllamaPromptExecutionSettings;

        Assert.NotNull(clone2);
        Assert.Null(clone2.Stop);
        Assert.Null(clone2.ExtensionData);
    }
}
