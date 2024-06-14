namespace SemanticKernel.Connectors.Hunyuan.UnitTests.TencentSdk;

public class HunyuanClientCoreTests
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<ClientProfile> _mockClientProfile;

    public HunyuanClientCoreTests()
    {
        this._mockLogger = new Mock<ILogger>();
        this._mockClientProfile = new Mock<ClientProfile>();
    }

    [Fact]
    public void ConstructorWorksCorrectly()
    {
        HunyuanClientCore core = new(TestConstants.FakeModel, TestConstants.FakeCredential, TestConstants.FakeRegion, this._mockClientProfile.Object, _mockLogger.Object);
        Assert.NotNull(core);
    }
}