using NSubstitute;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;
using TechLanchesLambdaTest.UnitTests.Fixtures;

namespace TechLanchesLambdaTest.UnitTests.Services
{
    [Trait("Services", "Cognito")]
    public class CognitoServiceTest : IClassFixture<CognitoServiceFixture>
    {
        private readonly ICognitoService _cognitoService;
        private readonly CognitoServiceFixture _cognitoServiceFixture;

        public CognitoServiceTest(CognitoServiceFixture cognitoServiceFixture)
        {
            _cognitoService = Substitute.For<ICognitoService>();
            _cognitoServiceFixture = cognitoServiceFixture;
        }

        [Fact(DisplayName = "Sign up de usuário inexistente com sucesso")]
        public async Task SignUp_UsuarioInexistente_DeveRetornarUsuarioCadastradoComSucesso()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Ok()));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Sucesso);
        }

        [Fact(DisplayName = "Sign up de usuário techlanches com sucesso")]
        public async Task SignUp_UsuarioTechLanches_DeveRetornarUsuarioCadastradoComSucesso()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuarioTechLanches();
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Ok()));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Sucesso);
        }

        [Fact(DisplayName = "Sign up de usuário inexistente com falha")]
        public async Task SignUp_UsuarioInexistenteEHouveFalhaNoSignUp_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Falha(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_autorizado_cadastro"))));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_autorizado_cadastro"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário inexistente e status code diferente de Ok com falha")]
        public async Task SignUp_UsuarioInexistenteEStatusCodeDiferenteDeOk_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Falha(_cognitoServiceFixture.ObterMensagemFalha("status_code_diferente_ok"))));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("status_code_diferente_ok"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário existente com falha")]
        public async Task SignUp_UsuarioExistente_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Falha(_cognitoServiceFixture.ObterMensagemFalha("usuario_cadastrado"))));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_cadastrado"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário inexistente com falha na confirmação")]
        public async Task SignUp_UsuarioInexistenteEHouveFalhaNaConfirmacao_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Falha(_cognitoServiceFixture.ObterMensagemFalha("falha_ao_confirmar_usuario"))));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("falha_ao_confirmar_usuario"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário cadastrado com sucesso")]
        public async Task SignIn_UsuarioCadastrado_DeveRetornarAutenticacaoComSucesso()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Nome;
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Ok(_cognitoServiceFixture.GerarToken())));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Value);
            Assert.NotNull(result.Value.AccessToken);
            Assert.NotNull(result.Value.TokenId);
            Assert.NotEmpty(result.Value.AccessToken);
            Assert.NotEmpty(result.Value.TokenId);
        }

        [Fact(DisplayName = "Sign in de usuário não autorizado com falha")]
        public async Task SignIn_UsuarioNaoAutorizado_DeveRetornarFalha()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Cpf; 
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_autorizado"))));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert

            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_autorizado"), result.Notificacoes.First().Mensagem);
        }

        #region Realizamos a auto confirmação do usuário, ou seja esse cenário nunca ocorrerá
        [Fact(DisplayName = "Sign in de usuário não confirmado com falha")]
        public async Task SignIn_UsuarioNaoConfirmado_DeveRetornarFalha()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Cpf;
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_confirmado"))));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_confirmado"), result.Notificacoes.First().Mensagem);
        }
        #endregion

        [Fact(DisplayName = "Sign in de usuário não encontrado com falha")]
        public async Task SignIn_UsuarioNaoEncontrado_DeveRetornarFalha()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Cpf;
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_encontrado"))));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert

            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_encontrado"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário com falha no sign in")]
        public async Task SignIn_HouveFalhaNoSignIn_DeveRetornarFalha()
        {
            // Arrange
            var userName = "517.625.890-01";
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>(_cognitoServiceFixture.ObterMensagemFalha("usuario_invalido"))));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_invalido"), result.Notificacoes.First().Mensagem);
        }
    }
}
