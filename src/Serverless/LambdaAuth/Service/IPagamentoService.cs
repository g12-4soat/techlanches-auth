using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Service
{
    public interface IPagamentoService
    {
        Task<Resultado> InativarDadosUsuarioPagamento(string cpf, string token);
    }
}
