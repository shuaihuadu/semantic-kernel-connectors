using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection;

// These examples show how to use HttpClient and HttpClientFactory within SK SDK.
public class HttpClient_Registration(ITestOutputHelper output) : BaseTest(output)
{
    /// <summary>
    /// Demonstrates the "basic usage" approach for HttpClientFactory.
    /// </summary>
    [Fact]
    public void UseBasicRegistrationWithHttpClientFactory()
    {
        //More details - https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#basic-usage
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();

        var kernel = serviceCollection.AddTransient<Kernel>((sp) =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();

            return Kernel.CreateBuilder()
                .AddOllamaChatCompletion(model: TestConfiguration.Ollama.ModelId, httpClient: factory.CreateClient())
                .Build();
        });
    }

    /// <summary>
    /// Demonstrates the "named clients" approach for HttpClientFactory.
    /// </summary>
    [Fact]
    public void UseNamedRegistrationWitHttpClientFactory()
    {
        // More details https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#named-clients

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();

        //Registration of a named HttpClient.
        serviceCollection.AddHttpClient("test-client", (client) =>
        {
            client.BaseAddress = new Uri(TestConfiguration.Ollama.Endpoint, UriKind.Absolute);
        });

        var kernel = serviceCollection.AddTransient<Kernel>((sp) =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();

            return Kernel.CreateBuilder()
                .AddOllamaChatCompletion(model: TestConfiguration.Ollama.ModelId, httpClient: factory.CreateClient())
                .Build();
        });
    }
}
