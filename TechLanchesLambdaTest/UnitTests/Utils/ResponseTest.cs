using System.Net;
using TechLanchesLambda.Utils;
using TechLanchesLambdaTest.UnitTests.Fixtures;

namespace TechLanchesLambdaTest.UnitTests.Utils
{
    [Trait("Utils", "Response")]
    public class ResponseTest : IClassFixture<UtilsFixture>
    {
        private readonly UtilsFixture _utilsFixture;

        public ResponseTest(UtilsFixture utilsFixture)
        {
            _utilsFixture = utilsFixture;
        }

        [Fact(DisplayName = "Response Bad Request ao receber lista notificações efetuado com sucesso")]
        public void ResponseBadRequest_ListaNotificacoes_DeveRetornarApiGatewayProxyResponseComSucesso()
        {
            // Arrange
            var notificacoes = _utilsFixture.GerarListaNotificacoes();

            //// Act
            var res = Response.BadRequest(notificacoes);

            //// Assert
            Assert.IsType<int>(res.StatusCode);
            Assert.True(res.StatusCode == (int)HttpStatusCode.BadRequest);
            Assert.IsType<string>(res.Body);
            Assert.NotEmpty(res.Body);
            Assert.NotNull(res.Body);
        }

        [Fact(DisplayName = "Response Bad Request ao receber string efetuado com sucesso")]
        public void ResponseBadRequest_String_DeveRetornarApiGatewayProxyResponseComSucesso()
        {
            // Arrange
            var mensagem = _utilsFixture.GerarMensagemNotificacao();

            //// Act
            var res = Response.BadRequest(mensagem);

            //// Assert
            Assert.IsType<int>(res.StatusCode);
            Assert.True(res.StatusCode == (int)HttpStatusCode.BadRequest);
            Assert.IsType<string>(res.Body);
            Assert.NotEmpty(res.Body);
            Assert.NotNull(res.Body);
        }

        [Fact(DisplayName = "Response Ok efetuado com sucesso")]
        public void ResponseOk_DeveRetornarApiGatewayProxyResponseComSucesso()
        {
            // Arrange
            var body = _utilsFixture.GerarTokenDto();

            //// Act
            var res = Response.Ok(body);

            //// Assert
            Assert.IsType<int>(res.StatusCode);
            Assert.True(res.StatusCode == (int)HttpStatusCode.OK);
            Assert.IsType<string>(res.Body);
            Assert.NotEmpty(res.Body);
            Assert.NotNull(res.Body);
        }
    }
}
