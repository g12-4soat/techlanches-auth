using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using TechLanchesLambda.AWS.Options;
using TechLanchesLambda.AWS.SecretsManager;
using TechLanchesLambda.Service;
using AWSOptions = TechLanchesLambda.AWS.Options.AWSOptions;

namespace TechLanchesLambda;

[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json")
            .AddAmazonSecretsManager("us-east-1", "lambda-auth-credentials");

        var configuration = builder.Build();

        services.AddSingleton<IConfiguration>(configuration);

        services.Configure<AWSOptions>(configuration);

        services.AddLogging();

        services.AddAuthentication();

        services.AddAuthorization();

        services.AddCognitoIdentity();

        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<ICognitoService, CognitoService>();
        
        services.AddScoped<ICognitoService>(x =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var opt = serviceProvider.GetRequiredService<IOptions<AWSOptions>>();

            var provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(opt.Value.Region));
            var client = new AmazonCognitoIdentityProviderClient();

            return new CognitoService(opt, client, provider);
        });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMvc();

        app.UseCors();

        app.UseAuthentication();

        app.UseAuthorization();
    }
}
