using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechLanches.Inativacao.DynamoDB.Repositories;
using TechLanchesLambda.Controller;
using TechLanchesLambda.Gateways;
using TechLanchesLambda.Presenter;
using TechLanchesLambda.Service;
using AWSOptions = TechLanchesLambda.AWS.Options.AWSOptions;

namespace TechLanchesLambda.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IUsuarioInativoPresenter, UsuarioInativoPresenter>();
            services.AddSingleton<IUsuarioInativoController, UsuarioInativoController>();
            services.AddSingleton<IUsuarioInativoGateway, UsuarioInativoGateway>();
            services.AddSingleton<IUsuarioInativoRepository, UsuarioInativoRepository>();

            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<ICognitoService, CognitoService>();

            services.AddScoped<IPagamentoService>(x =>
            {
                var httpCliente = new HttpClient();
                return new PagamentoService(httpCliente);
            });

            services.AddScoped<IPedidoService>(x =>
            {
                var httpCliente = new HttpClient();
                return new PedidoService(httpCliente);
            });

            services.AddScoped<ICognitoService>(x =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var opt = serviceProvider.GetRequiredService<IOptions<AWSOptions>>();

                var provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(opt.Value.Region));
                var client = new AmazonCognitoIdentityProviderClient();

                return new CognitoService(opt, client, provider);
            });

            services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var config = new AmazonDynamoDBConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                };
                return new AmazonDynamoDBClient(config);
            });
        }
    }
}
