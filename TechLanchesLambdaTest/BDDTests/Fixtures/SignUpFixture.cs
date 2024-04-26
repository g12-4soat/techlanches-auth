using TechLanchesLambda.DTOs;

namespace TechLanchesLambdaTest.BDDTests.Fixtures
{
    public class SignUpFixture : IDisposable
    {
        private const string MENSAGEM_USUARIO_CADASTRADO = "Usuário já cadastrado. Por favor tente autenticar.";
        private const string MENSAGEM_USUARIO_NAO_AUTORIZADO = "Usuário não autorizado para cadastro com os dados informados.";
        private const string MENSAGEM_STATUS_CODE_DIFERENTE_OK = "Houve algo de errado ao cadastrar o usuário.";
        private const string MENSAGEM_FALHA_AO_CONFIRMAR_USUARIO = "Não foi possível confirmar o usuário.";

        public SignUpFixture() { }

        public UsuarioDto GerarUsuario()
        {
            return new UsuarioDto { Cpf = "958.315.760-00", Email = "test@example.com", Nome = "Test User" };
        }

        public UsuarioDto GerarUsuarioTechLanches()
        {
            return new UsuarioDto { Cpf = "techlanches", Email = "techlanches@example.com", Nome = "techlanches" };
        }

        public string ObterMensagemFalha(string nomeMensagem)
        {
            switch (nomeMensagem.ToLower())
            {
                case "usuario_cadastrado": return MENSAGEM_USUARIO_CADASTRADO;
                case "usuario_nao_autorizado": return MENSAGEM_USUARIO_NAO_AUTORIZADO;
                case "status_code_diferente_ok": return MENSAGEM_STATUS_CODE_DIFERENTE_OK;
                case "falha_ao_confirmar_usuario": return MENSAGEM_FALHA_AO_CONFIRMAR_USUARIO;
                default: return MENSAGEM_USUARIO_NAO_AUTORIZADO;
            }
        }

        public void Dispose()
        {
        }
    }
}
