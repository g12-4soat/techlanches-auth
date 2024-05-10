using TechLanchesLambda.Utils;

namespace TechLanchesLambdaTest.UnitTests.Utils
{
    [Trait("Utils", "ValidatorCPF")]
    public class ValidatorCPFTest
    {
        [Fact(DisplayName = "Validador de CPF efetuado com sucesso")]
        public void ValidatorCPF_Validar_DeveRetornarCpfComSucesso()
        {
            // Arrange
            string fakeCpf = "900.293.200-62";

            //// Act
            var res = ValidatorCPF.Validar(fakeCpf);

            //// Assert
            Assert.True(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Validador de CPF efetuado com falha")]
        public void ValidatorCPF_Validar_DeveRetornarCpfComFalha()
        {
            // Arrange
            string fakeCpf = "000.293.200-62";

            //// Act
            var res = ValidatorCPF.Validar(fakeCpf);

            //// Assert
            Assert.False(res);
            Assert.IsType<bool>(res);
        }

        [Fact(DisplayName = "Limpar CPF efetuado com sucesso")]
        public void LimparCPF_Limpar_DeveRetornarCpfLimpoComSucesso()
        {
            // Arrange
            string fakeCpf = "900.293.200-62";
            string fakeCpfLimpo = "90029320062";

            //// Act
            var res = ValidatorCPF.LimparCpf(fakeCpf);

            //// Assert
            Assert.Equal(res, fakeCpfLimpo);
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
