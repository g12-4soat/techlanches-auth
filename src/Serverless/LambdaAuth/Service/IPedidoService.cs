using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Service
{
    public interface IPedidoService
    {
        Task<Resultado> InativarDadosUsuarioPedido(string cpf, string token);
    }
}
