namespace SemanticKernel.Connectors.Hunyuan.UnitTests;

[ExcludeFromCodeCoverage]
internal static class HunyuanTestHelper
{
    internal static class Constants
    {
        internal const string FakeModel = "fake-model";
        internal const string FakeRegion = "fake-region";
        internal const string FakeSecretId = "secretId";
        internal const string FakeSecretKey = "SecretKey";
        internal const string FakeToken = "Token";
        internal const string FakeUriString = "http://fake.url";

        internal static readonly Credential FakeCredential = new()
        {
            SecretId = FakeSecretId,
            SecretKey = FakeSecretKey,
            Token = FakeToken
        };
        internal static readonly ClientProfile FakeClientProfile = new Mock<ClientProfile>().Object;
        internal static readonly ILogger FakeLogger = new Mock<ILogger>().Object;
    }

    internal static string GetTestResponse(string fileName)
    {
        return File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "TestData", fileName));
    }

    internal static void SetTestHttpClient<T>(T service, HttpClient httpClient) where T : IAIService
    {
        HunyuanClient hunyuanClient = new(TestConstants.FakeCredential, TestConstants.FakeRegion)
        {
            HttpClient = httpClient
        };

        HunyuanClientCore core = new(
            TestConstants.FakeModel,
            TestConstants.FakeCredential,
            TestConstants.FakeRegion,
            TestConstants.FakeClientProfile,
            TestConstants.FakeLogger);

        Type coreType = core.GetType();

        FieldInfo? clientFieldInfo = coreType.GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance);

        clientFieldInfo?.SetValue(core, hunyuanClient);

        Type serviceType = service.GetType();

        FieldInfo? corefieldInfo = serviceType.GetField("_core", BindingFlags.NonPublic | BindingFlags.Instance);

        corefieldInfo?.SetValue(service, core);
    }
}
