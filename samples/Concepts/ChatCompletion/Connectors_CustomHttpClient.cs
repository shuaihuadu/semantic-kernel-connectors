namespace ChatCompletion;

public class Connectors_CustomHttpClient(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void UseCustomHttpClient()
    {
        using HttpClient httpClient = new()
        {
            BaseAddress = new Uri(TestConfiguration.Ollama.Endpoint),
            Timeout = TimeSpan.FromSeconds(1200)
        };

        Kernel kernel = Kernel.CreateBuilder()
            .AddOllamaTextGeneration(TestConfiguration.Ollama.ModelId, httpClient)
            .Build();
    }
}
