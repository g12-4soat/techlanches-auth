using Amazon.CognitoIdentityProvider.Model;
using TechLanchesLambda.DTOs;

namespace TechLanchesLambdaTest.UnitTests.Fixtures
{
    public class CognitoServiceFixture : IDisposable
    {
        private const string MENSAGEM_USUARIO_NAO_CONFIRMADO = "Usuário não confirmado.";
        private const string MENSAGEM_USUARIO_NAO_AUTORIZADO = "Usuário não autorizado com os dados informados.";
        private const string MENSAGEM_USUARIO_NAO_ENCONTRADO = "Usuário não encontrado com os dados informados.";
        private const string MENSAGEM_USUARIO_INVALIDO = "Ocorreu um erro ao fazer loginn.";
        private const string MENSAGEM_USUARIO_CADASTRADO = "Usuário já cadastrado. Por favor tente autenticar.";
        private const string MENSAGEM_USUARIO_NAO_AUTORIZADO_CADASTRO = "Usuário não autorizado para cadastro com os dados informadoss.";
        private const string MENSAGEM_STATUS_CODE_DIFERENTE_OK = "Houve algo de errado ao cadastrar o usuário.";
        private const string MENSAGEM_FALHA_AO_CONFIRMAR_USUARIO = "Não foi possível confirmar o usuárioo.";
        private const string MENSAGEM_USUARIO_NAO_AUTORIZADO_INATIVACAO = "Usuário não autorizado com os dados informados para inativação.";
        private const string MENSAGEM_STATUS_CODE_DIFERENTE_OK_INATIVACAO = "Houve algo de errado ao inativar o usuário.";


        public UsuarioDto GerarUsuario()
            => new UsuarioDto { Cpf = "958.315.760-00", Email = "test@example.com", Nome = "Test User" };

        public UsuarioDto GerarUsuarioLimpo()
            => new UsuarioDto { Cpf = "95831576000", Email = "test@example.com", Nome = "Test User" };

        public UsuarioDto GerarUsuarioTechLanches()
            => new UsuarioDto { Cpf = "techlanches", Email = "techlanches@example.com", Nome = "techlanches" };

        public TokenDto GerarToken()
        {
            var authResultType = new AuthenticationResultType()
            {
                IdToken = "lTgRzbKewhYrLUCTE7CCsWJOPP7avkKXWLcwhLja8p9IGjmsiXfy6LeJft5smCHH",
                AccessToken = "AtjOELlJfZQmSoPKYTvJutLX9iA5rRarpe9Oy9sd0lbyR2tAH3RWDr2zSjcCuAIl"
            };

            return new TokenDto(authResultType.IdToken, authResultType.AccessToken);
        }

        public OptionsDto GerarOptionsAws()
            => new OptionsDto
                            {
                                Region = "us-east-1",
                                UserPoolId = "us-east-1_dFgU6KROw",
                                UserPoolClientId = "3nts1rf3ejvc1lm788ro83qr8j",
                                PasswordDefault = "$Testt@1235$#1;",
                                EmailDefault = "teste.xpto@gmail.com",
                                UserTechLanches = "techlanches"
                            };

        public string ObterMensagemFalha(string nomeMensagem)
        {
            switch (nomeMensagem.ToLower())
            {
                case "usuario_nao_confirmado": return MENSAGEM_USUARIO_NAO_CONFIRMADO;
                case "usuario_nao_autorizado": return MENSAGEM_USUARIO_NAO_AUTORIZADO;
                case "usuario_nao_encontrado": return MENSAGEM_USUARIO_NAO_ENCONTRADO;
                case "usuario_invalido": return MENSAGEM_USUARIO_INVALIDO;
                case "usuario_cadastrado": return MENSAGEM_USUARIO_CADASTRADO;
                case "usuario_nao_autorizado_cadastro": return MENSAGEM_USUARIO_NAO_AUTORIZADO_CADASTRO;
                case "status_code_diferente_ok": return MENSAGEM_STATUS_CODE_DIFERENTE_OK;
                case "falha_ao_confirmar_usuario": return MENSAGEM_FALHA_AO_CONFIRMAR_USUARIO;
                case "usuario_nao_autorizado_inativacao": return MENSAGEM_USUARIO_NAO_AUTORIZADO_INATIVACAO;
                case "status_code_diferente_ok_inativacao": return MENSAGEM_STATUS_CODE_DIFERENTE_OK_INATIVACAO;
                default: return MENSAGEM_USUARIO_INVALIDO;
            }
        }

        public void Dispose()
        {
        }
    }
}
