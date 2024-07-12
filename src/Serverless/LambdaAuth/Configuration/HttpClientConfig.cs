using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Configuration;

public static class HttpClientConfig
{
    public static void AddHttpClientConfiguration(this IServiceCollection services)
    {
        //Criar uma politica de retry (tente 3x, com timeout de 3 segundos)
        var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                          .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1));

        //Registrar httpclient
        services.AddHttpClient<IPedidoService, PedidoService>(Constants.NOME_API_PEDIDO, httpClient =>
        {
            var url = Environment.GetEnvironmentVariable("PEDIDO_SERVICE")!;
            httpClient.BaseAddress = new Uri("http://" + url + ":5050");

        }).AddPolicyHandler(retryPolicy);
        services.AddHttpClient<IPagamentoService, PagamentoService>(Constants.NOME_API_PAGAMENTOS, httpClient =>
        {
            var url = Environment.GetEnvironmentVariable("PAGAMENTO_SERVICE")!;
            httpClient.BaseAddress = new Uri("http://" + url + ":5055");

        }).AddPolicyHandler(retryPolicy);
    }
}
