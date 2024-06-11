using Microsoft.SemanticKernel.ImageToText;
using Resources;

namespace ImageToText;

/// <summary>
/// Represents a class that demonstrates image-to-text functionality.
/// </summary>
public sealed class Ollama_ImageToText(ITestOutputHelper output) : BaseTest(output)
{
    private const string ImageToTextModel = "llava";
    private const string ImageFilePath = "test_image.jpg";

    [Fact]
    public async Task ImageToTextAsync()
    {
        // Create a kernel with Ollama image-to-text service
        var kernel = Kernel.CreateBuilder()
            .AddOllamaImageToText(
                model: ImageToTextModel,
                endpoint: TestConfiguration.Ollama.Endpoint)
            .Build();

        var imageToText = kernel.GetRequiredService<IImageToTextService>();

        // Set execution settings (optional)
        OllamaPromptExecutionSettings executionSettings = new()
        {
            MaxTokens = 500
        };

        // Read image content from a file
        ReadOnlyMemory<byte> imageData = await EmbeddedResource.ReadAllAsync(ImageFilePath);
        ImageContent imageContent = new(new BinaryData(imageData), "image/jpeg");

        // Convert image to text
        TextContent textContent = await imageToText.GetTextContentAsync(imageContent, executionSettings);

        // Output image description
        Console.WriteLine(textContent.Text);
    }
}
