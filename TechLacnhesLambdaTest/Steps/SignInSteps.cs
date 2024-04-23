using Amazon.CognitoIdentityProvider.Model;
using NSubstitute;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;

namespace TechLanchesLambdaTest.Steps
{
    [Binding]
    public class SignInSteps
    {
        private UsuarioDto _usuario;
        private Resultado<TokenDto> _resultado;
        private readonly ICognitoService _cognitoService;

        public SignInSteps()
        {
            _cognitoService = Substitute.For<ICognitoService>();
        }

        [Given(@"que os dados do usuário são válido")]
        public void GivenQueOsDadosDoUsuarioSaoValido()
        {
            //_usuario = table.CreateInstance<UsuarioDto>();

            _usuario = new UsuarioDto { Cpf = "958.315.760-00", Email = "test@example.com", Nome = "Test User" };
        }

        [When(@"autentica o usuário")]
        public async Task WhenAutenticaOUsuario()
        {
            var authResultType = new AuthenticationResultType();
            authResultType.IdToken = "lTgRzbKewhYrLUCTE7CCsWJOPP7avkKXWLcwhLja8p9IGjmsiXfy6LeJft5smCHH";
            authResultType.AccessToken = "AtjOELlJfZQmSoPKYTvJutLX9iA5rRarpe9Oy9sd0lbyR2tAH3RWDr2zSjcCuAIl";

            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Ok(new TokenDto(authResultType.IdToken, authResultType.AccessToken)));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        [When(@"autentica o usuário não confirmado")]
        public async Task WhenAutenticaOUsuarioNaoConfirmado()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>("Usuário não confirmado."));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        [When(@"autentica o usuário não autorizado")]
        public async Task WhenAutenticaOUsuarioNaoAutorizado()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>("Usuário não autorizado com os dados informados."));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        [When(@"autentica o usuário não encontrado")]
        public async Task WhenAutenticaOUsuarioNaoEncontrado()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>("Usuário não encontrado com os dados informados."));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        [When(@"autentica o usuário inválido")]
        public async Task WhenAutenticaOUsuarioInvalido()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>("Ocorreu um erro ao fazer login."));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        [Then(@"retorna o resultado sucesso na autenticação")]
        public void ThenRetornaOResultadoSucessoNaAutenticacao()
        {
            Assert.True(_resultado.Sucesso);
            Assert.NotNull(_resultado.Value);
            Assert.NotNull(_resultado.Value.AccessToken);
            Assert.NotNull(_resultado.Value.TokenId);
            Assert.NotEmpty(_resultado.Value.AccessToken);
            Assert.NotEmpty(_resultado.Value.TokenId);
        }

        [Then(@"retorna o resultado falha por usuário não confirmado")]
        public void ThenRetornaOResultadoFalhaPorUsuarioNaoConfirmado()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Usuário não confirmado.", _resultado.Notificacoes.First().Mensagem);
        }

        [Then(@"retorna o resultado falha por usuário não autorizado")]
        public void ThenRetornaOResultadoFalhaPorUsuarioNaoAutorizado()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Usuário não autorizado com os dados informados.", _resultado.Notificacoes.First().Mensagem);
        }

        [Then(@"retorna o resultado falha por usuário não encontrado")]
        public void ThenRetornaOResultadoFalhaPorUsuarioNaoEncontrado()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Usuário não encontrado com os dados informados.", _resultado.Notificacoes.First().Mensagem);
        }

        [Then(@"retorna o resultado falha na autenticação")]
        public void ThenRetornaOResultadoFalhaNaAutenticacao()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Ocorreu um erro ao fazer login.", _resultado.Notificacoes.First().Mensagem);
        }
    }
}
