using TechLanchesLambda.Service;
using System.Net;
using TechLanchesLambdaTest.UnitTests.Fixtures;
using TechLanchesLambdaTest.UnitTests.Utils;
using TechLanchesLambda.Utils;

namespace TechLanchesLambdaTest.UnitTests.Services
{
    [Trait("Services", "Pagamento")]
    public class PagamentoServiceTest : IClassFixture<PagamentoServiceFixture>
    {
        private readonly PagamentoServiceFixture _pagamentoServiceFixture;

        public PagamentoServiceTest(PagamentoServiceFixture pagamentoServiceFixture)
        {
            _pagamentoServiceFixture = pagamentoServiceFixture;
        }

        [Fact(DisplayName = "Inativar dados de pagamento do usuário com sucesso")]
        public async Task Inativar_DadosUsuarioPagamento_DeveRetornarSucesso()
        {
            // Arrange
            var cpf = _pagamentoServiceFixture.GerarCpf();
            var token = _pagamentoServiceFixture.GerarToken();

            var mockHttpMessageHandler = new MockHttpMessageHandler(String.Empty, HttpStatusCode.OK);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler);
            mockHttpClient.BaseAddress = new Uri($"http://localhost:5050/");

            var pagamentoService = new PagamentoService(mockHttpClient);

            // Act
            var resultado = await pagamentoService.InativarDadosUsuarioPagamento(cpf, token);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Notificacoes.Count == 0);
        }

        [Fact(DisplayName = "Inativar dados de pagamento do usuário com falha")]
        public async Task Inativar_DadosUsuarioPagamento_DeveRetornarFalha()
        {
            // Arrange
            var cpf = _pagamentoServiceFixture.GerarCpf();
            var token = _pagamentoServiceFixture.GerarToken();

            var mockHttpMessageHandler = new MockHttpMessageHandler(String.Empty, HttpStatusCode.BadRequest);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler);
            mockHttpClient.BaseAddress = new Uri($"http://localhost:5050/");

            var pagamentoService = new PagamentoService(mockHttpClient);

            // Act
            var resultado = await pagamentoService.InativarDadosUsuarioPagamento(cpf, token);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains(_pagamentoServiceFixture.ObterMensagemFalha("inativar_usuario"), resultado.Notificacoes.FirstOrDefault().Mensagem);
        }

        [Fact(DisplayName = "Inativar dados de pagamento do usuário com exceção")]
        public async Task Inativar_DadosUsuarioPagamento_DeveRetornarExcecao()
        {
            // Arrange
            var cpf = _pagamentoServiceFixture.GerarCpf();
            var token = _pagamentoServiceFixture.GerarToken();

            var mockHttpMessageHandler = new MockHttpMessageHandler(new HttpRequestException());
            var httpClient = new HttpClient(mockHttpMessageHandler);

            var pagamentoService = new PagamentoService(httpClient);

            // Act
            var resultado = await pagamentoService.InativarDadosUsuarioPagamento(cpf, token);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(_pagamentoServiceFixture.ObterMensagemFalha("inativar_usuario_excecao"), resultado.Notificacoes.FirstOrDefault().Mensagem);
        }
    }
}
