using TechLanches.Inativacao.DynamoDB.Models;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Gateways;

namespace TechLanchesLambda.UseCases
{
    public class UsuarioInativoUseCase
    {
        public static async Task<UsuarioInativoDbModel> Cadastrar(UsuarioInativoDbModel usuarioInativacao, IUsuarioInativoGateway usuarioInativoGateway)
        {
            await VerificarInativacaoExistente(usuarioInativacao.Cpf, usuarioInativoGateway);
            return await usuarioInativoGateway.Cadastrar(usuarioInativacao);
        }

        private static async Task<UsuarioInativoDbModel> VerificarInativacaoExistente(string cpf, IUsuarioInativoGateway usuarioInativoGateway)
        {
            var usuarioInativo = await usuarioInativoGateway.BuscarUsuarioInativoPorCpf(cpf);

            if (usuarioInativo is not null)
                throw new Exception($"Solicitação de inativação de usuário já cadastrado para o cpf: {cpf}");

            return usuarioInativo;
        }
    }
}
