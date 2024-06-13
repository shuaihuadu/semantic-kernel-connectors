namespace SemanticKernel.Connectors.Hunyuan.UnitTests.TencentSdk;

public class HunyuanClientCoreTests
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<Credential> _mockCredential;
    private readonly Mock<ClientProfile> _mockClientProfile;

    public HunyuanClientCoreTests()
    {
        this._mockLogger = new Mock<ILogger>();
        this._mockCredential = new Mock<Credential>();
        this._mockClientProfile = new Mock<ClientProfile>();
    }

    [Fact]
    public void ConstructorWorksCorrectly()
    {
        HunyuanClientCore core = new(TestConstants.FakeModel, this._mockCredential.Object, TestConstants.FakeRegion, this._mockClientProfile.Object, _mockLogger.Object);
        Assert.NotNull(core);
    }
}