using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TechLanchesLambda.Service;
using TechLanchesLambdaTest.UnitTests.Fixtures;

namespace TechLanchesLambdaTest.UnitTests.Services
{
    [Trait("Services", "Cognito")]
    public class CognitoServiceTest : IClassFixture<CognitoServiceFixture>
    {
        private readonly CognitoServiceFixture _cognitoServiceFixture;
        private readonly IOptions<TechLanchesLambda.AWS.Options.AWSOptions> _awsOptions;
        private readonly IAmazonCognitoIdentityProvider _client;
        private readonly IAmazonCognitoIdentityProvider _provider;

        public CognitoServiceTest(CognitoServiceFixture cognitoServiceFixture)
        {
            _awsOptions = Options.Create(new TechLanchesLambda.AWS.Options.AWSOptions());
            _client = Substitute.For<IAmazonCognitoIdentityProvider>();
            _provider = Substitute.For<IAmazonCognitoIdentityProvider>();
            _cognitoServiceFixture = cognitoServiceFixture;
            _awsOptions.Value.UserPoolId = _cognitoServiceFixture.GerarOptionsAws().UserPoolId;
            _awsOptions.Value.UserPoolClientId = _cognitoServiceFixture.GerarOptionsAws().UserPoolClientId;
            _awsOptions.Value.PasswordDefault = _cognitoServiceFixture.GerarOptionsAws().PasswordDefault;
            _awsOptions.Value.EmailDefault = _cognitoServiceFixture.GerarOptionsAws().EmailDefault;
            _awsOptions.Value.Region = _cognitoServiceFixture.GerarOptionsAws().Region;
            _awsOptions.Value.UserTechLanches = _cognitoServiceFixture.GerarOptionsAws().UserTechLanches;
        }

        [Fact(DisplayName = "Sign up de usuário inexistente com sucesso")]
        public async Task SignUp_UsuarioInexistente_DeveRetornarUsuarioCadastradoComSucesso()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();
            _awsOptions.Value.UserTechLanches = _cognitoServiceFixture.GerarUsuarioTechLanches().Cpf;

            _client.AdminGetUserAsync(Arg.Any<AdminGetUserRequest>()).Throws(new UserNotFoundException("user nao existe"));
            _client.SignUpAsync(Arg.Any<SignUpRequest>()).Returns(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });
            _client.AdminConfirmSignUpAsync(Arg.Any<AdminConfirmSignUpRequest>()).Returns(new AdminConfirmSignUpResponse());

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);

            // Act
            var result = await cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Sucesso);
        }

        [Fact(DisplayName = "Sign up de usuário techlanches com sucesso")]
        public async Task SignUp_UsuarioTechLanches_DeveRetornarUsuarioCadastradoComSucesso()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuarioTechLanches();
            _awsOptions.Value.UserTechLanches = user.Cpf;

            _client.AdminGetUserAsync(Arg.Any<AdminGetUserRequest>()).Returns(new AdminGetUserResponse());
            _client.SignUpAsync(Arg.Any<SignUpRequest>()).Returns(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });
            _client.AdminConfirmSignUpAsync(Arg.Any<AdminConfirmSignUpRequest>()).Returns(new AdminConfirmSignUpResponse());

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);

            // Act
            var result = await cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Sucesso);
        }

        [Fact(DisplayName = "Sign up de usuário inexistente com falha")]
        public async Task SignUp_UsuarioInexistenteEHouveFalhaNoSignUp_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();

            _client.AdminGetUserAsync(Arg.Any<AdminGetUserRequest>()).Throws(new UserNotFoundException(""));
            _client.SignUpAsync(Arg.Any<SignUpRequest>()).Throws(new NotAuthorizedException(""));
            _client.AdminConfirmSignUpAsync(Arg.Any<AdminConfirmSignUpRequest>()).Returns(new AdminConfirmSignUpResponse());

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);

            // Act
            var result = await cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_autorizado_cadastro"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário inexistente e status code diferente de Ok com falha")]
        public async Task SignUp_UsuarioInexistenteEStatusCodeDiferenteDeOk_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();

            _client.AdminGetUserAsync(Arg.Any<AdminGetUserRequest>()).Throws(new UserNotFoundException(""));
            _client.SignUpAsync(Arg.Any<SignUpRequest>()).Returns(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.Unauthorized });

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);

            // Act
            var result = await cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("status_code_diferente_ok"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário existente com falha")]
        public async Task SignUp_UsuarioExistente_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();

            _client.AdminGetUserAsync(Arg.Any<AdminGetUserRequest>()).Returns(new AdminGetUserResponse());
            var cognitoService = new CognitoService(_awsOptions, _client, _provider);

            // Act
            var result = await cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_cadastrado"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign up de usuário inexistente com falha na confirmação")]
        public async Task SignUp_UsuarioInexistenteEHouveFalhaNaConfirmacao_DeveRetornarFalha()
        {
            // Arrange
            var user = _cognitoServiceFixture.GerarUsuario();

            _client.AdminGetUserAsync(Arg.Any<AdminGetUserRequest>()).Throws(new UserNotFoundException(""));
            _client.SignUpAsync(Arg.Any<SignUpRequest>()).Returns(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });
            _client.AdminConfirmSignUpAsync(Arg.Any<AdminConfirmSignUpRequest>()).Throws(new NotAuthorizedException(""));

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);

            // Act
            var result = await cognitoService.SignUp(user);

            // Assert
            Assert.True(result.Falhou);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("falha_ao_confirmar_usuario"), result.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário não autorizado com falha")]
        public async Task SignIn_UsuarioNaoAutorizado_DeveRetornarFalha()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Cpf;

            _provider.AdminInitiateAuthAsync(Arg.Any<AdminInitiateAuthRequest>())
                .ThrowsForAnyArgs(new NotAuthorizedException(""));

            // Act
            var cognitoService = new CognitoService(_awsOptions, _client, _provider);
            var resultado = await cognitoService.SignIn(userName);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_autorizado"), resultado.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário não confirmado com falha")]
        public async Task SignIn_UsuarioNaoConfirmado_DeveRetornarFalha()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Cpf;

            _provider.AdminInitiateAuthAsync(Arg.Any<AdminInitiateAuthRequest>())
                .ThrowsForAnyArgs(new UserNotConfirmedException(""));

            // Act
            var cognitoService = new CognitoService(_awsOptions, _client, _provider);
            var resultado = await cognitoService.SignIn(userName);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_confirmado"), resultado.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário não encontrado com falha")]
        public async Task SignIn_UsuarioNaoEncontrado_DeveRetornarFalha()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Cpf;

            _provider.AdminInitiateAuthAsync(Arg.Any<AdminInitiateAuthRequest>())
                .ThrowsForAnyArgs(new UserNotFoundException(""));

            // Act
            var cognitoService = new CognitoService(_awsOptions, _client, _provider);
            var resultado = await cognitoService.SignIn(userName);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(_cognitoServiceFixture.ObterMensagemFalha("usuario_nao_encontrado"), resultado.Notificacoes.First().Mensagem);
        }

        [Fact(DisplayName = "Sign in de usuário com falha no sign in")]
        public async Task SignIn_HouveFalhaNoSignIn_DeveRetornarFalha()
        {
            // Arrange
            var userName = _cognitoServiceFixture.GerarUsuario().Cpf;

            _provider.AdminInitiateAuthAsync(Arg.Any<AdminInitiateAuthRequest>())
                .ThrowsForAnyArgs(new Exception(""));

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);
            //cognitoService.SignIn(userName).Returns(Task.FromResult(Resultado.Falha<TokenDto>(_cognitoServiceFixture.ObterMensagemFalha("usuario_invalido"))));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await cognitoService.SignIn(userName));
        }

        #region Testes com erros ao chamar StartWithAdminNoSrpAuthAsync
        //[Fact(DisplayName = "Sign in de usuário cadastrado com sucesso")]
        //public async Task SignIn_UsuarioCadastrado_DeveRetornarAutenticacaoComSucesso()
        //{
        //    // Arrange
        //    var userName = _cognitoServiceFixture.GerarUsuario().Cpf;
        //    var idToken = "idToken";
        //    var accessToken = "accessToken";

        //    var authResponse = new AdminInitiateAuthResponse
        //    {
        //        AuthenticationResult = new AuthenticationResultType
        //        {
        //            IdToken = idToken,
        //            AccessToken = accessToken
        //        }
        //    };
        //    _provider.AdminInitiateAuthAsync(Arg.Any<AdminInitiateAuthRequest>())
        //        .ReturnsForAnyArgs(Task.FromResult(authResponse));

        //    var cognitoService = new CognitoService(_awsOptions, _client, _provider);

        //    // Act
        //    var resultado = await cognitoService.SignIn(userName);

        //    // Assert
        //    Assert.True(resultado.Sucesso);
        //    Assert.NotNull(resultado.Value);
        //    Assert.NotNull(resultado.Value.AccessToken);
        //    Assert.NotNull(resultado.Value.TokenId);
        //    Assert.NotEmpty(resultado.Value.AccessToken);
        //    Assert.NotEmpty(resultado.Value.TokenId);
        //    Assert.Equal(idToken, resultado.Value.TokenId);
        //    Assert.Equal(accessToken, resultado.Value.AccessToken);
        //}
        #endregion
    }
}
