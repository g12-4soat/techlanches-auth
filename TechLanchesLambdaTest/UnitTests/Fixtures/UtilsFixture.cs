using System.Text.RegularExpressions;
using TechLanchesLambda.DTOs;

namespace TechLanchesLambdaTest.UnitTests.Fixtures
{
    public class UtilsFixture : IDisposable
    {
        public List<NotificacaoDto> GerarListaNotificacoes()
        {
            return new List<NotificacaoDto>()
            {
                new NotificacaoDto("Mensagem de notificação teste"),
                new NotificacaoDto("Mensagem de notificação teste 2")
            }; 
        }

        public string GerarMensagemNotificacao()
            => "Mensagem de notificação teste";

        public TokenDto GerarTokenDto()
        {
            var tokenId = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var accesToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5b";

            return new TokenDto(tokenId, accesToken);
        }

        public string GerarCpfValido()
            => "900.293.200-62";

        public string GerarCpfValidoELimpo()
           => Regex.Replace("900.293.200-62", @"[^\d]", "");

        public string GerarCpfInvalido()
            => "000.293.200-62";

        public string GerarEmailValido()
            => "test.xpto@gmail.com";

        public string GerarEmailInvalido()
            => "test.xpto@gmail";

        public void Dispose()
        {
        }
    }
}
