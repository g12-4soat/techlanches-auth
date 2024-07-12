using Amazon.Lambda.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechLanchesLambda.AWS.SecretsManager;
using TechLanchesLambda.Configuration;
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

        //DI Abstraction
        services.AddDependencyInjectionConfiguration();

        //Setting mapster
        services.AddRegisterMapsConfiguration();

        //Setting http client
        services.AddHttpClientConfiguration();
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
