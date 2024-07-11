using TechLanchesLambda.Service;
using System.Net;
using TechLanchesLambdaTest.UnitTests.Fixtures;
using TechLanchesLambdaTest.UnitTests.Utils;

namespace TechLanchesLambdaTest.UnitTests.Services
{
    [Trait("Services", "Pedido")]
    public class PedidoServiceTest : IClassFixture<PedidoServiceFixture>
    {
        private readonly PedidoServiceFixture _pedidoServiceFixture;

        public PedidoServiceTest(PedidoServiceFixture pedidoServiceFixture)
        {
            _pedidoServiceFixture = pedidoServiceFixture;
        }

        [Fact(DisplayName = "Inativar dados de pedido do usuário com sucesso")]
        public async Task Inativar_DadosUsuarioPedido_DeveRetornarSucesso()
        {
            // Arrange
            var cpf = _pedidoServiceFixture.GerarCpf();
            var token = _pedidoServiceFixture.GerarToken();

            var mockHttpMessageHandler = new MockHttpMessageHandler(String.Empty, HttpStatusCode.OK);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler);
            mockHttpClient.BaseAddress = new Uri($"http://localhost:5050/");

            var pedidoService = new PedidoService(mockHttpClient);

            // Act
            var resultado = await pedidoService.InativarDadosUsuarioPedido(cpf, token);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Notificacoes.Count == 0);
        }

        [Fact(DisplayName = "Inativar dados de pedido do usuário com falha")]
        public async Task Inativar_DadosUsuarioPedido_DeveRetornarFalha()
        {
            // Arrange
            var cpf = _pedidoServiceFixture.GerarCpf();
            var token = _pedidoServiceFixture.GerarToken();

            var mockHttpMessageHandler = new MockHttpMessageHandler(String.Empty, HttpStatusCode.BadRequest);
            var mockHttpClient = new HttpClient(mockHttpMessageHandler);
            mockHttpClient.BaseAddress = new Uri($"http://localhost:5050/");

            var pedidoService = new PedidoService(mockHttpClient);

            // Act
            var resultado = await pedidoService.InativarDadosUsuarioPedido(cpf, token);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains(_pedidoServiceFixture.ObterMensagemFalha("inativar_usuario"), resultado.Notificacoes.FirstOrDefault().Mensagem);
        }

        [Fact(DisplayName = "Inativar dados de pedido do usuário com exceção")]
        public async Task Inativar_DadosUsuarioPedido_DeveRetornarExcecao()
        {
            // Arrange
            var cpf = _pedidoServiceFixture.GerarCpf();
            var token = _pedidoServiceFixture.GerarToken();

            var mockHttpMessageHandler = new MockHttpMessageHandler(new HttpRequestException());
            var httpClient = new HttpClient(mockHttpMessageHandler);

            var pedidoService = new PedidoService(httpClient);

            // Act
            var resultado = await pedidoService.InativarDadosUsuarioPedido(cpf, token);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(_pedidoServiceFixture.ObterMensagemFalha("inativar_usuario_excecao"), resultado.Notificacoes.FirstOrDefault().Mensagem);
        }
    }
}
