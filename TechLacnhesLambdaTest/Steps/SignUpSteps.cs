using NSubstitute;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;

namespace TechLanchesLambdaTest.Steps
{
    [Binding]
    public class SignUpSteps
    {
        private UsuarioDto _usuario;
        private UsuarioDto _usuarioTechLanches;
        private Resultado _resultado;
        private readonly ICognitoService _cognitoService;

        public SignUpSteps()
        {
            _cognitoService = Substitute.For<ICognitoService>();
            _usuarioTechLanches = new UsuarioDto { Cpf = "techlanches", Email = "techlanches@example.com", Nome = "techlanches" };
        }

        [Given(@"que os dados do usuário são válidos")]
        public void GivenQueOsDadosDoUsuarioSaoValidos()
        {
            _usuario = new UsuarioDto { Cpf = "958.315.760-00", Email = "test@example.com", Nome = "Test User" }; ;
        }

        [Given(@"seja diferente do usuário techlanches")]
        public void GivenSejaDiferenteDoUsuarioTechlanches()
        {
            _usuario.Should().NotBeEquivalentTo(_usuarioTechLanches);
        }

        [Given(@"seja existente")]
        public async Task GivenSejaExistente()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha("Usuário já cadastrado. Por favor tente autenticar."));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        [Given(@"seja inexistente")]
        public void GivenSejaInexistente()
        {
            _cognitoService.SignUp(_usuario).Returns(Task.FromResult(Resultado.Ok()));
        }

        [Given(@"o usuário esteja cadastrado")]
        public void GivenOUsuarioEstejaCadastrado()
        {
            _cognitoService.SignUp(_usuario).Returns(Task.FromResult(Resultado.Ok()));
        }

        [When(@"cadastra o usuário")]
        public void WhenCadastraOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Task.FromResult(Resultado.Ok()));
            
        }

        [When(@"confirma o usuário")]
        public async Task WhenConfirmaOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Ok());
            _resultado = await _cognitoService.SignUp(_usuario);

        }

        [When(@"houver falha ao cadastrar o usuário")]
        public async Task WhenHouverFalhaAoCadastrarOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha("Usuário não autorizado para cadastro com os dados informados."));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        [When(@"o retorno do cadastro de usuário é diferente do status code OK")]
        public async Task WhenORetornoDoCadastroDeUsuarioEDiferenteDoStatusCodeOK()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha("Houve algo de errado ao cadastrar o usuário."));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        [When(@"houver falha ao confirmar o usuário")]
        public async Task WhenHouverFalhaAoConfirmarOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha("Não foi possível confirmar o usuário."));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        [Then(@"retorna o resultado sucesso")]
        public void ThenRetornaOResultadoSucesso()
        {
            Assert.True(_resultado.Sucesso);
        }

        [Then(@"retorna o resultado falha")]
        public void ThenRetornaOResultadoFalha()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Usuário não autorizado para cadastro com os dados informados.", _resultado.Notificacoes.First().Mensagem);
        }

        [Then(@"retorna o resultado falha por status code")]
        public void ThenRetornaOResultadoFalhaPorStatusCode()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Houve algo de errado ao cadastrar o usuário.", _resultado.Notificacoes.First().Mensagem);
        }

        [Then(@"retorna o resultado falha de usuário existente")]
        public void ThenRetornaOResultadoFalhaDeUsuarioExistente()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Usuário já cadastrado. Por favor tente autenticar.", _resultado.Notificacoes.First().Mensagem);
        }
        
        [Then(@"retorna o resultado falha na confirmação do usuário")]
        public void ThenRetornaOResultadoFalhaNaConfirmacaoDoUsuario()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal("Não foi possível confirmar o usuário.", _resultado.Notificacoes.First().Mensagem);
        }
    }
}
