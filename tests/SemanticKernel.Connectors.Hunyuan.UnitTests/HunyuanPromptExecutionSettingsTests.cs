namespace SemanticKernel.Connectors.Hunyuan.UnitTests;

public class HunyuanPromptExecutionSettingsTests
{
    [Fact]
    public void FromExecutionSettingsWhenNewShouldReturnSame()
    {
        HunyuanPromptExecutionSettings executionSettings = new();

        HunyuanPromptExecutionSettings hunyuanPromptExecutionSettings = HunyuanPromptExecutionSettings.FromExecutionSettings(executionSettings);

        Assert.Same(executionSettings, hunyuanPromptExecutionSettings);
    }

    [Fact]
    public void FromExecutionSettingsWhenNullShouldReturnDefault()
    {
        HunyuanPromptExecutionSettings? executionSettings = null;

        HunyuanPromptExecutionSettings hunyuanPromptExecutionSettings = HunyuanPromptExecutionSettings.FromExecutionSettings(executionSettings);

        Assert.NotNull(hunyuanPromptExecutionSettings);
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

        HunyuanPromptExecutionSettings hunyuanPromptExecutionSettings = HunyuanPromptExecutionSettings.FromExecutionSettings(executionSettings);

        Assert.NotNull(hunyuanPromptExecutionSettings);
        Assert.Equal(executionSettings.ModelId, hunyuanPromptExecutionSettings.ModelId);
        Assert.Equal(executionSettings.ExtensionData["key"].ToString(), hunyuanPromptExecutionSettings!.ExtensionData!["key"].ToString());
    }

    [Fact]
    public void PromptExecutionSettingsCloneWorksAsExpected()
    {
        string configPayload = """
        {
            "Temperature": 0.5,
            "TopP": 0.4,
            "EnableEnhancement": true,
            "StreamModeration": false,
            "Stream": false,
            "ChatSystemPrompt":"Assistant is a large language model."
        }
        """;

        HunyuanPromptExecutionSettings? executionSettings = JsonSerializer.Deserialize<HunyuanPromptExecutionSettings>(configPayload);
        executionSettings!.ExtensionData = new Dictionary<string, object>
        {
            ["key1"] = 1
        };

        HunyuanPromptExecutionSettings? clone = executionSettings.Clone() as HunyuanPromptExecutionSettings;

        Assert.NotNull(clone);
        Assert.Equal(executionSettings.ModelId, clone.ModelId);
        Assert.Equivalent(executionSettings.ExtensionData, clone.ExtensionData);
        Assert.NotNull(clone.ExtensionData);
        Assert.NotEmpty(clone.ExtensionData);
        Assert.Equal(1, clone.ExtensionData["key1"]);
        Assert.Equal(executionSettings.Temperature, clone.Temperature);
        Assert.Equal(executionSettings.TopP, clone.TopP);
        Assert.Equal(executionSettings.EnableEnhancement, clone.EnableEnhancement);
        Assert.Equal(executionSettings.StreamModeration, clone.StreamModeration);
        Assert.Equal(executionSettings.Stream, clone.Stream);
        Assert.Equal(executionSettings.ChatSystemPrompt, clone.ChatSystemPrompt);
    }

    [Fact]
    public void PromptExecutionSettingsWithNullValuesCloneWorksAsExpected()
    {
        string configPayload = """
        {
            "Temperature": 0.5,
            "TopP": 0.4,
            "EnableEnhancement": true,
            "StreamModeration": false,
            "Stream": false,
            "ChatSystemPrompt":null
        }
        """;

        HunyuanPromptExecutionSettings? executionSettings = JsonSerializer.Deserialize<HunyuanPromptExecutionSettings>(configPayload);

        HunyuanPromptExecutionSettings? clone = executionSettings!.Clone() as HunyuanPromptExecutionSettings;

        Assert.NotNull(clone);
        Assert.Null(clone.ExtensionData);
        Assert.Equal("Assistant is a large language model.", clone.ChatSystemPrompt);
    }
}