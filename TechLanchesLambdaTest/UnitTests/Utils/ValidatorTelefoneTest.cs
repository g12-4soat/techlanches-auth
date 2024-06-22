using TechLanchesLambda.Validations;
using TechLanchesLambdaTest.UnitTests.Fixtures;

namespace TechLanchesLambdaTest.UnitTests.Utils
{
    [Trait("Utils", "ValidatorTelefone")]
    public class ValidatorTelefoneTest : IClassFixture<UtilsFixture>
    {
        private readonly UtilsFixture _utilsFixture;

        public ValidatorTelefoneTest(UtilsFixture utilsFixture)
        {
            _utilsFixture = utilsFixture;
        }

        [Fact(DisplayName = "Validador de telefone efetuado com sucesso")]
        public void ValidatorEmail_Validar_DeveRetornarTelefoneComSucesso()
        {
            // Arrange
            string telefone = _utilsFixture.GerarTelefoneValido();

            //// Act
            var res = ValidatorTelefone.Validar(telefone);

            //// Assert
            Assert.True(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de telefone inválido efetuado com falha")]
        public void ValidatorTelefonel_ValidarTelefoneInvalido_DeveRetornarTelefoneComFalha()
        {
            // Arrange
            string telefone = _utilsFixture.GerarTelefoneInvalido();

            //// Act
            var res = ValidatorTelefone.Validar(telefone);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de telefone vazio efetuado com falha")]
        public void ValidatorTelefone_ValidarTelefoneVazio_DeveRetornarTelefoneComFalha()
        {
            // Arrange
            string fakeTelefone = "";

            //// Act
            var res = ValidatorTelefone.Validar(fakeTelefone);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }
    }
}
