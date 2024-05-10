using TechLanchesLambda.Utils;

namespace TechLanchesLambdaTest.UnitTests.Utils
{
    [Trait("Utils", "ValidatorEmail")]
    public class ValidatorEmailTest
    {
        [Fact(DisplayName = "Validador de E-mail efetuado com sucesso")]
        public void ValidatorEmail_Validar_DeveRetornarEmailComSucesso()
        {
            // Arrange
            string fakeEmail = "test.xpto@gmail.com";

            //// Act
            var res = ValidatorEmail.Validar(fakeEmail);

            //// Assert
            Assert.True(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de E-mail inválido efetuado com falha")]
        public void ValidatorEmail_ValidarEmailInvalido_DeveRetornarEmailComFalha()
        {
            // Arrange
            string fakeEmail = "test.xpto@gmail";

            //// Act
            var res = ValidatorEmail.Validar(fakeEmail);

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
