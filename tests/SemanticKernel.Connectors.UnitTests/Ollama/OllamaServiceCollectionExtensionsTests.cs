namespace SemanticKernel.Connectors.UnitTests.Ollama;

public class OllamaServiceCollectionExtensionsTests
{
    #region Text Generation

    [Fact]
    public void AddOllamaTextGenerationWithUriStringToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeUriString);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextGenerationService service = serviceProvider.GetRequiredService<ITextGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextGenerationWithUriToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeUri);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextGenerationService service = serviceProvider.GetRequiredService<ITextGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextGenerationWithHttpClientToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextGenerationService service = serviceProvider.GetRequiredService<ITextGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextGenerationWithHttpClientWithNullBaseAddressToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
        Assert.Throws<ArgumentNullException>(() => services.AddOllamaTextGeneration(TestConstants.FakeModel, endpoint: default(Uri)));
        Assert.Throws<UriFormatException>(() => services.AddOllamaTextGeneration(TestConstants.FakeModel, string.Empty));
    }

    #endregion

    #region Chat Completion

    [Fact]
    public void AddOllamaChatCompletionWithUriStringToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeUriString);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IChatCompletionService service = serviceProvider.GetRequiredService<IChatCompletionService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }

    [Fact]
    public void AddOllamaChatCompletionWithUriToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeUri);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IChatCompletionService service = serviceProvider.GetRequiredService<IChatCompletionService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }

    [Fact]
    public void AddOllamaChatCompletionWitHttpClientToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IChatCompletionService service = serviceProvider.GetRequiredService<IChatCompletionService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }


    [Fact]
    public void AddOllamaChatCompletionWithHttpClientWithNullBaseAddressToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
        Assert.Throws<ArgumentNullException>(() => services.AddOllamaChatCompletion(TestConstants.FakeModel, endpoint: default(Uri)));
        Assert.Throws<UriFormatException>(() => services.AddOllamaChatCompletion(TestConstants.FakeModel, string.Empty));
    }

    #endregion

    #region Text Embedding

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithUriStringToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeUriString);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextEmbeddingGenerationService service = serviceProvider.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithUriToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeUri);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextEmbeddingGenerationService service = serviceProvider.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithHttpClientToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextEmbeddingGenerationService service = serviceProvider.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithHttpClientWithNullBaseAddressToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
        Assert.Throws<ArgumentNullException>(() => services.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, endpoint: default(Uri)));
        Assert.Throws<UriFormatException>(() => services.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, string.Empty));
    }

    #endregion

    #region Text to Image

    [Fact]
    public void AddOllamaTextToImageWithUriStringToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeUriString);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IImageToTextService service = serviceProvider.GetRequiredService<IImageToTextService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaImageToTextService>(service);
    }

    [Fact]
    public void AddOllamaTextToImageWithUriToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeUri);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IImageToTextService service = serviceProvider.GetRequiredService<IImageToTextService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaImageToTextService>(service);
    }

    [Fact]
    public void AddOllamaTextToImageWithHttpClientToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IImageToTextService service = serviceProvider.GetRequiredService<IImageToTextService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<OllamaImageToTextService>(service);
    }

    [Fact]
    public void AddOllamaTextToImageWithHttpClientWithNullBaseAddressToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
        Assert.Throws<ArgumentNullException>(() => services.AddOllamaImageToText(TestConstants.FakeModel, endpoint: default(Uri)));
        Assert.Throws<UriFormatException>(() => services.AddOllamaImageToText(TestConstants.FakeModel, string.Empty));
    }

    #endregion
}
