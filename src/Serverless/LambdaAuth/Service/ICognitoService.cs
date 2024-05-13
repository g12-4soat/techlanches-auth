using TechLanchesLambda.DTOs;
using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Service
{
    public interface ICognitoService
    {
        Task<Resultado> SignUp(UsuarioDto usuario);
        Task<Resultado<TokenDto>> SignIn(string userName);
    }
}
