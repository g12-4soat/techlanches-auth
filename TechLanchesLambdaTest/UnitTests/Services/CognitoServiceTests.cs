using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Reflection;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;

namespace TechLanchesLambdaTest.UnitTests.Services
{
    public class CognitoServiceTests
    {
        private readonly IOptions<TechLanchesLambda.AWS.Options.AWSOptions> _awsOptions;
        private readonly IAmazonCognitoIdentityProvider _client;
        private readonly IAmazonCognitoIdentityProvider _provider;

        public CognitoServiceTests()
        {
            _awsOptions = Options.Create(new TechLanchesLambda.AWS.Options.AWSOptions());
            _client = Substitute.For<IAmazonCognitoIdentityProvider>();
            _provider = Substitute.For<IAmazonCognitoIdentityProvider>();
        }

        [Fact]
        public async Task SignUp_UserAlreadyExists_ReturnsFailure()
        {
            // Arrange
            var userDto = new UsuarioDto { Cpf = "existingUser", Email = "existing@example.com", Nome = "Existing User" };
            _awsOptions.Value.UserTechLanches = "techlanches";

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);
            // Act
            var result = await cognitoService.SignUp(userDto);

            // Assert
            Assert.False(result.Sucesso);
        }

        [Fact]
        public async Task SignUp_UserDoesNotExist_ReturnsSuccess()
        {
            // Arrange
            var userDto = new UsuarioDto { Cpf = "newUser", Email = "new@example.com", Nome = "New User" };
            _awsOptions.Value.UserTechLanches = "techlanches";

            _client.SignUpAsync(Arg.Any<SignUpRequest>()).Returns(new SignUpResponse { HttpStatusCode = System.Net.HttpStatusCode.OK });
            _client.AdminGetUserAsync(Arg.Any<AdminGetUserRequest>()).Throws(new UserNotFoundException("user nao existe"));

            var cognitoService = new CognitoService(_awsOptions, _client, _provider);
            // Act
            var result = await cognitoService.SignUp(userDto);


            // Assert
            Assert.True(result.Sucesso);
        }

    }
}
