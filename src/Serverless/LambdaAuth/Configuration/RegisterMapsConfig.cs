using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechLanches.Inativacao.DynamoDB.Models;
using TechLanchesLambda.DTOs;

namespace TechLanchesLambda.Configuration
{
    public static class RegisterMapsConfig
    {
        public static void AddRegisterMapsConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<UsuarioInativoDbModel, UsuarioInativacaoDto>.NewConfig()
                .Map(dest => dest.Cpf, src => src.Cpf)
                .Map(dest => dest.Nome, src => src.Nome)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Endereco, src => src.Endereco)
                .Map(dest => dest.Telefone, src => src.Telefone);

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
