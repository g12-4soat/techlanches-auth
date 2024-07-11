using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net;
using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Configuration
{
    public static class HttpClientConfig
    {
        public static void AddHttpClientConfiguration(this IServiceCollection services)
        {
            //var teste = Environment.GetEnvironmentVariable("TESTE")!;

            //Console.WriteLine($"TESTE 01 => {teste}");

            var teste2 = Environment.GetEnvironmentVariable("PEDIDO_SERVICE")!;

            var teste3 = Environment.GetEnvironmentVariable("PAGAMENTO_SERVICE")!;

            Console.WriteLine($"PEDIDO_SERVICE DNS => {teste2}");
            Console.WriteLine($"PAGAMENTO_SERVICE DNS => {teste3}");

            services.AddHttpClient(Constants.PEDIDOS, httpClient =>
            {
                //var url = Environment.GetEnvironmentVariable("PEDIDO_SERVICE")!;

                //httpClient.BaseAddress = new Uri("http://" + url + ":5050");
            }).AddStandardResilienceHandler(options =>
            {
                options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError);
                options.Retry.MaxRetryAttempts = 5;
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);
            });

            ////Registrar httpclient
            services.AddHttpClient(Constants.PAGAMENTOS, httpClient =>
            {
                //var url = Environment.GetEnvironmentVariable("PAGAMENTO_SERVICE")!;

                //httpClient.BaseAddress = new Uri("http://" + teste3 + ":5055");
            }).AddStandardResilienceHandler(options =>
            {
                options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError);
                options.Retry.MaxRetryAttempts = 5;
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);
            });
        }
    }
}
