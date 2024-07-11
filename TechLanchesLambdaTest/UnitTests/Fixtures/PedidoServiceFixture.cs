namespace TechLanchesLambdaTest.UnitTests.Fixtures
{
    public class PedidoServiceFixture : IDisposable
    {
        private const string MENSAGEM_INATIVAR_USUARIO = "Erro durante chamada api de pedidos.";
        private const string MENSAGEM_INATIVAR_USUARIO_EXCECAO = "Falha ao ler arquivo string da api/pedidos/inativar";

        public string GerarCpf()
            => "95831576000";

        public string GerarToken()
            => "ChgBuYDUOsybAzdoIMPAB509FJHbhzmmixpHyPwwqBWqh2X3mIehg2yiIVKPV1mpf6sx9XkkjaWQ9uE2rfxuC4GL2kkZQ1F4H11ElJdZrKC9eiBqFfHsCRQpMGacbhSEFChiyVc5BUW0kObZm82UJkxwXUADKTL9iEUtWpT2KHXUJyDx1FMS7KhlKIbwDAuJvZcvwTgCCYskeGtdrg0KuHK2UYKu7st1cFQU8TcAoeW9ATSdUOVZMMRFbpoUG1MiDe2viETcUla0lw9Fi2EQKVK2EPPhtDxT5sXgivxG0VhS6tUllJ1LRPDfCwyOOK4qE9MWzv";

        public string GerarUrlPedido()
           => $"http://localhost:5050/api/pedidos/inativar/{GerarCpf()}";

        public string ObterMensagemFalha(string nomeMensagem)
        {
            switch (nomeMensagem.ToLower())
            {
                case "inativar_usuario": return MENSAGEM_INATIVAR_USUARIO;
                case "inativar_usuario_excecao": return MENSAGEM_INATIVAR_USUARIO_EXCECAO;
                default: return MENSAGEM_INATIVAR_USUARIO;
            }
        }

        public void Dispose()
        {
        }
    }
}
