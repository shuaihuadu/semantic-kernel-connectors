namespace SemanticKernel.Connectors.Hunyuan.UnitTests.Extensions;

public class HunyuanServiceCollectionExtensionsTests
{
    #region Text Generation

    [Fact]
    public void AddHunyuanTextGenerationToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddHunyuanTextGeneration(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextGenerationService service = serviceProvider.GetRequiredService<ITextGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<HunyuanChatCompletionService>(service);
    }

    #endregion

    #region Chat Completion

    [Fact]
    public void AddHunyuanChatCompletionToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddHunyuanChatCompletion(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IChatCompletionService service = serviceProvider.GetRequiredService<IChatCompletionService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<HunyuanChatCompletionService>(service);
    }

    #endregion

    #region Text Embedding

    [Fact]
    public void AddHunyuanTextEmbeddingGenerationToServiceCollection()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddHunyuanTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ITextEmbeddingGenerationService service = serviceProvider.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotEmpty(services);
        Assert.NotNull(service);

        Assert.IsType<HunyuanTextEmbeddingGenerationService>(service);
    }

    #endregion
}
