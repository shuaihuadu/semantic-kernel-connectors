namespace SemanticKernel.Connectors.Hunyuan.UnitTests.Extensions;

public class HunyuanKernelBuilderExtensionsTests
{
    #region Text Generation

    [Fact]
    public void AddHunyuanTextGenerationCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddHunyuanTextGeneration(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken);

        Kernel kernel = builder.Build();

        ITextGenerationService service = kernel.GetRequiredService<ITextGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<HunyuanChatCompletionService>(service);
    }

    #endregion

    #region Chat Completion

    [Fact]
    public void AddHunyuanChatCompletionWithCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddHunyuanChatCompletion(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken);

        Kernel kernel = builder.Build();

        IChatCompletionService service = kernel.GetRequiredService<IChatCompletionService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<HunyuanChatCompletionService>(service);
    }


    #endregion

    #region Text Embedding

    [Fact]
    public void AddHunyuanTextEmbeddingGenerationCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddHunyuanTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken);

        Kernel kernel = builder.Build();

        ITextEmbeddingGenerationService service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<HunyuanTextEmbeddingGenerationService>(service);
    }

    #endregion
}
