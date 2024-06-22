using TechLanchesLambda.Validations;
using TechLanchesLambdaTest.UnitTests.Fixtures;

namespace TechLanchesLambdaTest.UnitTests.Utils
{
    [Trait("Utils", "ValidatorCPF")]
    public class ValidatorCPFTest : IClassFixture<UtilsFixture>
    {
        private readonly UtilsFixture _utilsFixture;
        public ValidatorCPFTest(UtilsFixture utilsFixture)
        {
            _utilsFixture = utilsFixture;   
        }

        [Fact(DisplayName = "Validador de CPF efetuado com sucesso")]
        public void ValidatorCPF_Validar_DeveRetornarCpfComSucesso()
        {
            // Arrange
            string cpf = _utilsFixture.GerarCpfValido();

            //// Act
            var res = ValidatorCPF.Validar(cpf);

            //// Assert
            Assert.True(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de CPF efetuado com falha")]
        public void ValidatorCPF_Validar_DeveRetornarCpfComFalha()
        {
            // Arrange
            string cpf = _utilsFixture.GerarCpfInvalido();

            //// Act
            var res = ValidatorCPF.Validar(cpf);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de CPF diferente de 11 digitos efetuado com falha")]
        public void ValidatorCPF_ValidarDiferenteQtdDigitos_DeveRetornarCpfComFalha()
        {
            // Arrange
            string cpf = _utilsFixture.GerarCpfValido();
            cpf = cpf.Remove(1);

            //// Act
            var res = ValidatorCPF.Validar(cpf);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Limpar CPF efetuado com sucesso")]
        public void LimparCPF_Limpar_DeveRetornarCpfLimpoComSucesso()
        {
            // Arrange
            string cpf = _utilsFixture.GerarCpfValido();
            string cpfLimpo = _utilsFixture.GerarCpfValidoELimpo(); 

            //// Act
            var res = ValidatorCPF.LimparCpf(cpf);

            //// Assert
            Assert.Equal(res, cpfLimpo);
            Assert.IsType<string>(res);
            Assert.NotEmpty(res);
            Assert.NotNull(res);
        }

        [Fact(DisplayName = "Limpar CPF efetuado com falha")]
        public void LimparCPF_Limpar_DeveRetornarCpfLimpoComFalha()
        {
            // Arrange
            string fakeCpf = null;

            // Act
            var res = ValidatorCPF.LimparCpf(fakeCpf);

            // Assert
            Assert.Equal(res, fakeCpf);
            Assert.Null(res);
        }
    }
}
