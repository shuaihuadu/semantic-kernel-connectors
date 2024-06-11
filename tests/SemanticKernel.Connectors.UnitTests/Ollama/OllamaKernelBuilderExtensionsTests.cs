﻿using Microsoft.SemanticKernel.ImageToText;

namespace SemanticKernel.Connectors.UnitTests.Ollama;

public class OllamaKernelBuilderExtensionsTests
{
    #region Text Generation

    [Fact]
    public void AddOllamaTextGenerationWithUriStringCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeUriString);

        Kernel kernel = builder.Build();

        ITextGenerationService service = kernel.GetRequiredService<ITextGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextGenerationWithUriCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeUri);

        Kernel kernel = builder.Build();

        ITextGenerationService service = kernel.GetRequiredService<ITextGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextGenerationWithHttpClientCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Kernel kernel = builder.Build();

        ITextGenerationService service = kernel.GetRequiredService<ITextGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextGenerationWithHttpClientWithNullBaseAddressCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.AddOllamaTextGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
    }

    #endregion

    #region Chat Completion

    [Fact]
    public void AddOllamaChatCompletionWithUriStringCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeUriString);

        Kernel kernel = builder.Build();

        IChatCompletionService service = kernel.GetRequiredService<IChatCompletionService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }

    [Fact]
    public void AddOllamaChatCompletionWithUriCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeUri);

        Kernel kernel = builder.Build();

        IChatCompletionService service = kernel.GetRequiredService<IChatCompletionService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }

    [Fact]
    public void AddOllamaChatCompletionWitHttpClientCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Kernel kernel = builder.Build();

        IChatCompletionService service = kernel.GetRequiredService<IChatCompletionService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaChatCompletionService>(service);
    }


    [Fact]
    public void AddOllamaChatCompletionWithHttpClientWithNullBaseAddressCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.AddOllamaChatCompletion(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
    }

    #endregion

    #region Text Embedding

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithUriStringCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeUriString);

        Kernel kernel = builder.Build();

        ITextEmbeddingGenerationService service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithUriCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeUri);

        Kernel kernel = builder.Build();

        ITextEmbeddingGenerationService service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithHttpClientCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Kernel kernel = builder.Build();

        ITextEmbeddingGenerationService service = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaTextEmbeddingGenerationService>(service);
    }

    [Fact]
    public void AddOllamaTextEmbeddingGenerationWithHttpClientWithNullBaseAddressCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.AddOllamaTextEmbeddingGeneration(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
    }

    #endregion


    #region Text to Image

    [Fact]
    public void AddOllamaTextToImageWithUriStringCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeUriString);

        Kernel kernel = builder.Build();

        IImageToTextService service = kernel.GetRequiredService<IImageToTextService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaImageToTextService>(service);
    }

    [Fact]
    public void AddOllamaTextToImageWithUriCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeUri);

        Kernel kernel = builder.Build();

        IImageToTextService service = kernel.GetRequiredService<IImageToTextService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaImageToTextService>(service);
    }

    [Fact]
    public void AddOllamaTextToImageWithHttpClientCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        builder.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeHttpClient);

        Kernel kernel = builder.Build();

        IImageToTextService service = kernel.GetRequiredService<IImageToTextService>();

        Assert.NotNull(kernel);
        Assert.NotNull(service);

        Assert.IsType<OllamaImageToTextService>(service);
    }

    [Fact]
    public void AddOllamaTextToImageWithHttpClientWithNullBaseAddressCreatesService()
    {
        IKernelBuilder builder = Kernel.CreateBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.AddOllamaImageToText(TestConstants.FakeModel, TestConstants.FakeHttpClientWithNullBaseAddress));
    }

    #endregion
}
