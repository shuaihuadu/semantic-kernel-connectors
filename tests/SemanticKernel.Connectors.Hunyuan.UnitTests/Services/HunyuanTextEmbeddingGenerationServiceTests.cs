// Copyright (c) IdeaTech. All rights reserved.

namespace SemanticKernel.Connectors.Hunyuan.UnitTests.Services;

public class HunyuanTextEmbeddingGenerationServiceTests : IDisposable
{
    private readonly HttpMessageHandlerStub _messageHandlerStub;

    private readonly HttpClient _httpClient;

    private readonly Mock<ILoggerFactory> _mockLoggerFactory;

    public HunyuanTextEmbeddingGenerationServiceTests()
    {
        this._messageHandlerStub = new HttpMessageHandlerStub();
        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(HunyuanTestHelper.GetTestResponse("text_embedding_test_response.json"));
        this._httpClient = new HttpClient(this._messageHandlerStub, false);
        this._mockLoggerFactory = new Mock<ILoggerFactory>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConstructorWorksCorrectly(bool includeLoggerFactory)
    {
        HunyuanTextEmbeddingGenerationService hunyuanTextEmbeddingGenerationService = includeLoggerFactory
            ? new HunyuanTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, TestConstants.FakeToken, loggerFactory: this._mockLoggerFactory.Object)
            : new HunyuanTextEmbeddingGenerationService(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey, 10, TestConstants.FakeRegion, loggerFactory: this._mockLoggerFactory.Object);

        Assert.NotNull(hunyuanTextEmbeddingGenerationService);
        Assert.Equal(TestConstants.FakeModel, hunyuanTextEmbeddingGenerationService.Attributes["ModelId"]);
    }

    [Fact]
    public async Task ShouldHandleServiceResponseAsync()
    {
        HunyuanTextEmbeddingGenerationService hunyuanTextEmbeddingGenerationService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey);

        HunyuanTestHelper.SetTestHttpClient(hunyuanTextEmbeddingGenerationService, this._httpClient);

        IList<ReadOnlyMemory<float>> embeddings = await hunyuanTextEmbeddingGenerationService.GenerateEmbeddingsAsync(["hello"]);

        Assert.NotNull(embeddings);
        Assert.Single(embeddings);
        Assert.Equal(1024, embeddings[0].Length);
    }

    [Fact]
    public async Task ShouldThrowKernelWhenEmbeddingIsNullAsync()
    {
        HunyuanTextEmbeddingGenerationService hunyuanTextEmbeddingGenerationService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey);

        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(HunyuanTestHelper.GetTestResponse("text_embedding_test_invalid_response1.json"));

        HunyuanTestHelper.SetTestHttpClient(hunyuanTextEmbeddingGenerationService, this._httpClient);

        KernelException exception = await Assert.ThrowsAsync<KernelException>(async () => await hunyuanTextEmbeddingGenerationService.GenerateEmbeddingsAsync(["hello"]));

        Assert.Equal("The result of the embedding is null or empty.", exception.Message);
    }

    [Fact]
    public async Task ShouldThrowKernelWhenEmbeddingElementIsNullAsync()
    {
        HunyuanTextEmbeddingGenerationService hunyuanTextEmbeddingGenerationService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey);

        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(HunyuanTestHelper.GetTestResponse("text_embedding_test_invalid_response2.json"));

        HunyuanTestHelper.SetTestHttpClient(hunyuanTextEmbeddingGenerationService, this._httpClient);

        KernelException exception = await Assert.ThrowsAsync<KernelException>(async () => await hunyuanTextEmbeddingGenerationService.GenerateEmbeddingsAsync(["hello"]));

        Assert.Equal("The result of the embedding contains null elements.", exception.Message);
    }

    [Fact]
    public async Task ShouldThrowKernelWhenEmbeddingCountNotEqualAsync()
    {
        HunyuanTextEmbeddingGenerationService hunyuanTextEmbeddingGenerationService = new(TestConstants.FakeModel, TestConstants.FakeSecretId, TestConstants.FakeSecretKey);

        this._messageHandlerStub.ResponseToReturn.Content = new StringContent(HunyuanTestHelper.GetTestResponse("text_embedding_test_invalid_response3.json"));

        HunyuanTestHelper.SetTestHttpClient(hunyuanTextEmbeddingGenerationService, this._httpClient);

        KernelException exception = await Assert.ThrowsAsync<KernelException>(async () => await hunyuanTextEmbeddingGenerationService.GenerateEmbeddingsAsync(["hello"]));

        Assert.Equal("Expected 1 text embedding(s), but received 2.", exception.Message);
    }

    public void Dispose()
    {
        this._httpClient.Dispose();
        this._messageHandlerStub.Dispose();
    }
}
