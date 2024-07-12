using NSubstitute;
using TechLanchesLambda.Validations;
using TechLanchesLambdaTest.UnitTests.Fixtures;

namespace TechLanchesLambdaTest.UnitTests.Utils
{
    [Trait("Utils", "ValidatorEmail")]
    public class ValidatorEmailTest : IClassFixture<UtilsFixture>
    {
        private readonly UtilsFixture _utilsFixture;

        public ValidatorEmailTest(UtilsFixture utilsFixture)
        {
            _utilsFixture = utilsFixture;
        }

        [Fact(DisplayName = "Validador de E-mail efetuado com sucesso")]
        public void ValidatorEmail_Validar_DeveRetornarEmailComSucesso()
        {
            // Arrange
            string email = _utilsFixture.GerarEmailValido();

            //// Act
            var res = ValidatorEmail.Validar(email);

            //// Assert
            Assert.True(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de E-mail inválido efetuado com falha")]
        public void ValidatorEmail_ValidarEmailInvalido_DeveRetornarEmailComFalha()
        {
            // Arrange
            string email = _utilsFixture.GerarEmailInvalido();

            //// Act
            var res = ValidatorEmail.Validar(email);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de E-mail vazio efetuado com falha")]
        public void ValidatorEmail_ValidarEmailVazio_DeveRetornarEmailComFalha()
        {
            // Arrange
            string fakeEmail = "";

            //// Act
            var res = ValidatorEmail.Validar(fakeEmail);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }
    }
}
