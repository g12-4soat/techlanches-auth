using Amazon.CognitoIdentityProvider.Model;
using NSubstitute;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;

namespace TechLanchesLambdaTest.Service
{
    [Trait("Services", "Cognito")]
    public class CognitoServiceTest
    {
        private readonly ICognitoService _cognitoService;

        public CognitoServiceTest()
        {
            _cognitoService = Substitute.For<ICognitoService>();
        }

        [Fact(DisplayName = "Sign up de usuário inexistente com sucesso")]
        public async Task SignUp_UsuarioInexistente_DeveRetornarUsuarioCadastradoComSucesso()
        {
            // Arrange
            var user = new UsuarioDto { Cpf = "958.315.760-00", Email = "test@example.com", Nome = "Test User" };
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
            var user = new UsuarioDto { Cpf = "techlanches", Email = "techlanches@example.com", Nome = "techlanches" };
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
            var user = new UsuarioDto { Cpf = "828.730.570-50", Email = "test@example.com", Nome = "Test User" };
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Falha("Usuário não autorizado para cadastro com os dados informados.")));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal("Usuário não autorizado para cadastro com os dados informados.", result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário inexistente e status code diferente de Ok com falha")]
        public async Task SignUp_UsuarioInexistenteEStatusCodeDiferenteDeOk_DeveRetornarFalha()
        {
            // Arrange
            var user = new UsuarioDto { Cpf = "828.730.570-50", Email = "test@example.com", Nome = "Test User" };
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Falha("Houve algo de errado ao cadastrar o usuário.")));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal("Houve algo de errado ao cadastrar o usuário.", result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário existente com falha")]
        public async Task SignUp_UsuarioExistente_DeveRetornarFalha()
        {
            // Arrange
            var user = new UsuarioDto { Cpf = "958.315.760-00", Email = "test@example.com", Nome = "Test User" };
            _cognitoService.SignUp(user).Returns(Task.FromResult(Resultado.Falha("Usuário já cadastrado. Por favor tente autenticar.")));

            // Act
            var result = await _cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal("Usuário já cadastrado. Por favor tente autenticar.", result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário cadastrado com sucesso")]
        public async Task SignIn_UsuarioCadastrado_DeveRetornarAutenticacaoComSucesso()
        {
            // Arrange
            var userName = "techlanches";

            var authResultType = new AuthenticationResultType();
            authResultType.IdToken = "lTgRzbKewhYrLUCTE7CCsWJOPP7avkKXWLcwhLja8p9IGjmsiXfy6LeJft5smCHH";
            authResultType.AccessToken = "AtjOELlJfZQmSoPKYTvJutLX9iA5rRarpe9Oy9sd0lbyR2tAH3RWDr2zSjcCuAIl";
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Ok(new TokenDto(authResultType.IdToken, authResultType.AccessToken))));

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
            var userName = "517.625.890-01";
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>("Usuário não autorizado com os dados informados.")));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert

            Assert.True(result.Falhou);
            Assert.Equal("Usuário não autorizado com os dados informados.", result.Notificacoes.First().Mensagem);
        }

        #region Realizamos a auto confirmação do usuário, ou seja esse cenário nunca ocorrerá
        [Fact(DisplayName = "Sign in de usuário não confirmado com falha")]
        public async Task SignIn_UsuarioNaoConfirmado_DeveRetornarFalha()
        {
            // Arrange
            var userName = "517.625.890-01";
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>("Usuário não confirmado.")));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal("Usuário não confirmado.", result.Notificacoes.First().Mensagem);
        }
        #endregion

        [Fact(DisplayName = "Sign in de usuário não encontrado com falha")]
        public async Task SignIn_UsuarioNaoEncontrado_DeveRetornarFalha()
        {
            // Arrange
            var userName = "517.625.890-01";
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>("Usuário não encontrado com os dados informados.")));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert

            Assert.True(result.Falhou);
            Assert.Equal("Usuário não encontrado com os dados informados.", result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário com falha no sign in")]
        public async Task SignIn_HouveFalhaNoSignIn_DeveRetornarFalha()
        {
            // Arrange
            var userName = "517.625.890-01";
            _cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>("Ocorreu um erro ao fazer login.")));

            // Act
            var result = await _cognitoService.SignIn(userName);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal("Ocorreu um erro ao fazer login.", result.Notificacoes.First().Mensagem);
        }
    }
}