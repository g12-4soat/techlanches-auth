using NSubstitute;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;
using TechLanchesLambdaTest.BDDTests.Fixtures;

namespace TechLanchesLambdaTest.BDDTests.Steps
{
    public class SignInSteps : IClassFixture<SignInFixture>
    {
        private UsuarioDto _usuario;
        private Resultado<TokenDto> _resultado;
        private readonly ICognitoService _cognitoService;
        private readonly SignInFixture _signInFixture;

        public SignInSteps(SignInFixture signInFixture)
        {
            _cognitoService = Substitute.For<ICognitoService>();
            _signInFixture = signInFixture;
        }

        [Fact(DisplayName = "Autenticação de usuário")]
        [Trait("SignIn|Cenário", "Autenticando um usuário")]
        public async Task AutenticandoUsuario()
        {
            GivenQueOsDadosDoUsuarioSaoValido();
            await WhenAutenticaOUsuario();
            ThenRetornaOResultadoSucessoNaAutenticacao();
        }

        [Fact(DisplayName = "Falha na autenticação de usuário não confirmado")]
        [Trait("SignIn|Cenário", "Falha ao tentar autenticar um usuário não confirmado")]
        public async Task FalhaAutenticacaoUsuarioNaoConfirmado()
        {
            GivenQueOsDadosDoUsuarioSaoValido();
            await WhenAutenticaOUsuarioNaoConfirmado();
            ThenRetornaOResultadoFalhaPorUsuarioNaoConfirmado();
        }

        [Fact(DisplayName = "Falha na autenticação de usuário não autorizado")]
        [Trait("SignIn|Cenário", "Falha ao tentar autenticar um usuário não autorizado")]
        public async Task FalhaAutenticacaoUsuarioNaoAutorizado()
        {
            GivenQueOsDadosDoUsuarioSaoValido();
            await WhenAutenticaOUsuarioNaoAutorizado();
            ThenRetornaOResultadoFalhaPorUsuarioNaoAutorizado();
        }

        [Fact(DisplayName = "Falha na autenticação de usuário não encontrado")]
        [Trait("SignIn|Cenário", "Falha ao tentar autenticar um usuário não encontrado")]
        public async Task FalhaAutenticacaoUsuarioNaoEncontrado()
        {
            GivenQueOsDadosDoUsuarioSaoValido();
            await WhenAutenticaOUsuarioNaoEncontrado();
            ThenRetornaOResultadoFalhaPorUsuarioNaoEncontrado();
        }

        [Fact(DisplayName = "Autenticação de usuário inválido")]
        [Trait("SignIn|Cenário", "Retorno da autenticação de usuário inválido")]
        public async Task AutenticacaoUsuarioInvalido()
        {
            GivenQueOsDadosDoUsuarioSaoValido();
            await WhenAutenticaOUsuarioInvalido();
            ThenRetornaOResultadoFalhaNaAutenticacao();
        }

        private void GivenQueOsDadosDoUsuarioSaoValido()
        {
            _usuario = _signInFixture.GerarUsuario();
        }

        private async Task WhenAutenticaOUsuario()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Ok(_signInFixture.GerarToken()));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        private async Task WhenAutenticaOUsuarioNaoConfirmado()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>(_signInFixture.ObterMensagemFalha("usuario_nao_confirmado")));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        private async Task WhenAutenticaOUsuarioNaoAutorizado()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>(_signInFixture.ObterMensagemFalha("usuario_nao_autorizado")));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        private async Task WhenAutenticaOUsuarioNaoEncontrado()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>(_signInFixture.ObterMensagemFalha("usuario_nao_encontrado")));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        private async Task WhenAutenticaOUsuarioInvalido()
        {
            _cognitoService.SignIn(_usuario.Nome).Returns(Resultado.Falha<TokenDto>(_signInFixture.ObterMensagemFalha("usuario_invalido")));
            _resultado = await _cognitoService.SignIn(_usuario.Nome);
        }

        private void ThenRetornaOResultadoSucessoNaAutenticacao()
        {
            Assert.True(_resultado.Sucesso);
            Assert.NotNull(_resultado.Value);
            Assert.NotNull(_resultado.Value.AccessToken);
            Assert.NotNull(_resultado.Value.TokenId);
            Assert.NotEmpty(_resultado.Value.AccessToken);
            Assert.NotEmpty(_resultado.Value.TokenId);
        }

        private void ThenRetornaOResultadoFalhaPorUsuarioNaoConfirmado()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signInFixture.ObterMensagemFalha("usuario_nao_confirmado"), _resultado.Notificacoes.First().Mensagem);
        }

        private void ThenRetornaOResultadoFalhaPorUsuarioNaoAutorizado()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signInFixture.ObterMensagemFalha("usuario_nao_autorizado"), _resultado.Notificacoes.First().Mensagem);
        }

        private void ThenRetornaOResultadoFalhaPorUsuarioNaoEncontrado()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signInFixture.ObterMensagemFalha("usuario_nao_encontrado"), _resultado.Notificacoes.First().Mensagem);
        }

        private void ThenRetornaOResultadoFalhaNaAutenticacao()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signInFixture.ObterMensagemFalha("usuario_invalido"), _resultado.Notificacoes.First().Mensagem);
        }
    }
}
