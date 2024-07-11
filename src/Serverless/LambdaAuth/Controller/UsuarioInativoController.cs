using TechLanchesLambda.DTOs;
using TechLanchesLambda.Gateways;
using TechLanchesLambda.Presenter;
using TechLanchesLambda.UseCases;
using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Controller
{
    public interface IUsuarioInativoController
    {
        Task<Resultado> Cadastrar(UsuarioInativacaoDto usuarioInativacao);
    }

    public class UsuarioInativoController : IUsuarioInativoController
    {
        private readonly IUsuarioInativoGateway _usuarioInativoGateway;
        private readonly IUsuarioInativoPresenter _usuarioInativoPresenter;

        public UsuarioInativoController(IUsuarioInativoGateway usuarioInativoGateway, IUsuarioInativoPresenter usuarioInativoPresenter)
        {
            _usuarioInativoGateway = usuarioInativoGateway;
            _usuarioInativoPresenter = usuarioInativoPresenter;
        }

        public async Task<Resultado> Cadastrar(UsuarioInativacaoDto usuarioInativacao)
        {
            try
            {
                var usuarioInativoModel = _usuarioInativoPresenter.ParaModel(usuarioInativacao);

                var res = await UsuarioInativoUseCase.Cadastrar(usuarioInativoModel, _usuarioInativoGateway);

                return res is not null ? Resultado.Ok() : Resultado.Falha("Falha ao realizar a solicitação de inativação do usuário.");
            }
            catch(Exception ex)
            {
                return Resultado.Falha($"Solicitação de inativação de usuário falhou com a seguinte mensagem: {ex.Message}");
            }
        }
    }
}
