using NSubstitute;
using System.Text;
using TechLanchesLambda.Utils;
using TechLanchesLambda.Service;
using System.Net;
using TechLanchesLambdaTest.UnitTests.Fixtures;

namespace TechLanchesLambdaTest.UnitTests.Services
{
    [Trait("Services", "Pagamento")]
    public class PagamentoServiceTest : IClassFixture<PagamentoServiceFixture>
    {
        private readonly PagamentoServiceFixture _pagamentoServiceFixture;
        //private readonly IHttpClientFactory _httpClient;

        public PagamentoServiceTest(PagamentoServiceFixture pagamentoServiceFixture)
        {
            //_httpClient = Substitute.For<IHttpClientFactory>().CreateClient(Constants.PAGAMENTOS);
            _pagamentoServiceFixture = pagamentoServiceFixture;
        }

        [Fact(DisplayName = "Inativar dados de pagamento do usuário com sucesso")]
        public async Task Inativar_DadosUsuarioPagamento_DeveRetornarSucesso()
        {
            // Arrange
            var cpf = _pagamentoServiceFixture.GerarCpf();
            var token = _pagamentoServiceFixture.GerarToken();
            var pagamentoUrl = _pagamentoServiceFixture.GerarUrlPagamento();

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClient = Substitute.For<HttpClient>();

            httpClientFactory.CreateClient(Constants.PAGAMENTOS).Returns(httpClient);

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
            };

            httpClient
                .PostAsync(Arg.Is<string>(uri => uri == pagamentoUrl), Arg.Any<StringContent>())
                .Returns(Task.FromResult(httpResponseMessage));

            var service = new PagamentoService(httpClientFactory);

            // Act
            var resultado = await service.InativarDadosUsuarioPagamento(cpf, token);

            // Assert
            Assert.Equal(Resultado.Ok(), resultado);
        }

        [Fact(DisplayName = "Inativar dados de pagamento do usuário com falha")]
        public async Task Inativar_DadosUsuarioPagamento_DeveRetornarFalha()
        {
            // Arrange
            var cpf = _pagamentoServiceFixture.GerarCpf();
            var token = _pagamentoServiceFixture.GerarToken();
            var pagamentoUrl = _pagamentoServiceFixture.GerarUrlPagamento();

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClient = Substitute.For<HttpClient>();

            httpClientFactory.CreateClient(Constants.PAGAMENTOS).Returns(httpClient);

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
            };

            httpClient
                .PostAsync(Arg.Is<string>(uri => uri == pagamentoUrl), Arg.Any<StringContent>())
                .Returns(Task.FromResult(httpResponseMessage));

            var service = new PagamentoService(httpClientFactory);

            // Act
            var resultado = await service.InativarDadosUsuarioPagamento(cpf, token);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains(_pagamentoServiceFixture.ObterMensagemFalha("inativar_usuario"), resultado.Notificacoes.FirstOrDefault().Mensagem);
        }
    }
}
