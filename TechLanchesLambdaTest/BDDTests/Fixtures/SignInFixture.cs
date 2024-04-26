using Amazon.CognitoIdentityProvider.Model;
using TechLanchesLambda.DTOs;

namespace TechLanchesLambdaTest.BDDTests.Fixtures
{
    public class SignInFixture : IDisposable
    {
        private const string MENSAGEM_USUARIO_NAO_CONFIRMADO = "Usuário não confirmado.";
        private const string MENSAGEM_USUARIO_NAO_AUTORIZADO = "Usuário não autorizado com os dados informados.";
        private const string MENSAGEM_USUARIO_NAO_ENCONTRADO = "Usuário não encontrado com os dados informados.";
        private const string MENSAGEM_USUARIO_INVALIDO = "Ocorreu um erro ao fazer login.";

        public SignInFixture() { }

        public UsuarioDto GerarUsuario()
        {
            return new UsuarioDto { Cpf = "958.315.760-00", Email = "test@example.com", Nome = "Test User" };
        }

        public UsuarioDto GerarUsuarioTechLanches()
        {
            return new UsuarioDto { Cpf = "techlanches", Email = "techlanches@example.com", Nome = "techlanches" };
        }

        public TokenDto GerarToken()
        {
            var authResultType = new AuthenticationResultType()
            {
                IdToken = "lTgRzbKewhYrLUCTE7CCsWJOPP7avkKXWLcwhLja8p9IGjmsiXfy6LeJft5smCHH",
                AccessToken = "AtjOELlJfZQmSoPKYTvJutLX9iA5rRarpe9Oy9sd0lbyR2tAH3RWDr2zSjcCuAIl"
            };

            return new TokenDto(authResultType.IdToken, authResultType.AccessToken);
        }

        public string ObterMensagemFalha(string nomeMensagem)
        {
            switch (nomeMensagem.ToLower())
            {
                case "usuario_nao_confirmado": return MENSAGEM_USUARIO_NAO_CONFIRMADO;
                case "usuario_nao_autorizado": return MENSAGEM_USUARIO_NAO_AUTORIZADO;
                case "usuario_nao_encontrado": return MENSAGEM_USUARIO_NAO_ENCONTRADO;
                case "usuario_invalido": return MENSAGEM_USUARIO_INVALIDO;
                default: return MENSAGEM_USUARIO_INVALIDO;
            }
        }

        public void Dispose()
        {
        }
    }
}
