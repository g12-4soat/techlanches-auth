using TechLanches.Inativacao.DynamoDB.Models;
using TechLanches.Inativacao.DynamoDB.Repositories;

namespace TechLanchesLambda.Gateways
{
    public interface IUsuarioInativoGateway
    {
        Task<UsuarioInativoDbModel> Cadastrar(UsuarioInativoDbModel usuarioInativo);
        Task<UsuarioInativoDbModel> BuscarUsuarioInativoPorCpf(string cpf);
    }

    public class UsuarioInativoGateway : IUsuarioInativoGateway
    {
        private readonly IUsuarioInativoRepository _usuarioInativoRepository;

        public UsuarioInativoGateway(IUsuarioInativoRepository usuarioInativoRepository)
        {
            _usuarioInativoRepository = usuarioInativoRepository;
        }

        public Task<UsuarioInativoDbModel> Cadastrar(UsuarioInativoDbModel usuarioInativo)
            => _usuarioInativoRepository.Cadastrar(usuarioInativo);

        public Task<UsuarioInativoDbModel> BuscarUsuarioInativoPorCpf(string cpf)
           => _usuarioInativoRepository.BuscarUsuarioInativoPorCpf(cpf);
    }
}
