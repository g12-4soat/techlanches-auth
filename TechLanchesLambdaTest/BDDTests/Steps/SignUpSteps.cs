using FluentAssertions;
using NSubstitute;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;
using TechLanchesLambdaTest.BDDTests.Fixtures;

namespace TechLanchesLambdaTest.BDDTests.Steps
{
    public class SignUpSteps : IClassFixture<SignUpFixture>
    {
        private Resultado _resultado;
        private UsuarioDto _usuario;
        private UsuarioDto _usuarioTechLanches;
        private readonly ICognitoService _cognitoService;
        private readonly SignUpFixture _signUpFixture;

        public SignUpSteps(SignUpFixture signUpFixture)
        {
            _signUpFixture = signUpFixture;
            _cognitoService = Substitute.For<ICognitoService>();
            _usuarioTechLanches = _signUpFixture.GerarUsuarioTechLanches();
        }

        [Fact(DisplayName = "Cadastro de usuário")]
        [Trait("SignUp|Cenário", "Cadastrando um usuário")]
        public async Task CadastrandoUsuario()
        {
            GivenQueOsDadosDoUsuarioSaoValidos();
            GivenSejaDiferenteDoUsuarioTechlanches();
            GivenSejaInexistente();
            WhenCadastraOUsuario();
            await WhenConfirmaOUsuario();
            ThenRetornaOResultadoSucesso();
        }

        [Fact(DisplayName = "Cadastro do usuário techlanches")]
        [Trait("SignUp|Cenário", "Cadastrando usuário techlanches")]
        public async Task CadastrandoUsuarioTechLanches()
        {
            GivenQueOUsuarioETechLanches();
            GivenSejaInexistente();
            WhenCadastraOUsuario();
            await WhenConfirmaOUsuario();
            ThenRetornaOResultadoSucesso();
        }

        [Fact(DisplayName = "Usuário Existente")]
        [Trait("SignUp|Cenário", "Usuário já cadastrado")]
        public async Task UsuarioJaCadastrado()
        {
            GivenQueOsDadosDoUsuarioSaoValidos();
            GivenSejaDiferenteDoUsuarioTechlanches();
            await GivenSejaExistente();
            WhenCadastraOUsuario();
            ThenRetornaOResultadoFalhaDeUsuarioExistente();
        }

        [Fact(DisplayName = "Cadastro de usuário inválido")]
        [Trait("SignUp|Cenário", "Retorno do cadastro de usuário inválido")]
        public async Task RetornaCadastroDeUsuarioInvalido()
        {
            GivenQueOsDadosDoUsuarioSaoValidos();
            GivenSejaDiferenteDoUsuarioTechlanches();
            await GivenSejaExistente();
            await WhenORetornoDoCadastroDeUsuarioEDiferenteDoStatusCodeOK();
            ThenRetornaOResultadoFalhaPorStatusCode();
        }

        [Fact(DisplayName = "Falha no cadastro de usuário")]
        [Trait("SignUp|Cenário", "Falha ao tentar cadastrar usuário")]
        public async Task FalhaAoTentarCadastrarUsuario()
        {
            GivenQueOsDadosDoUsuarioSaoValidos();
            GivenSejaDiferenteDoUsuarioTechlanches();
            await GivenSejaExistente();
            await WhenHouverFalhaAoCadastrarOUsuario();
            ThenRetornaOResultadoFalha();
        }

        [Fact(DisplayName = "Falha na confirmação de usuário")]
        [Trait("SignUp|Cenário", "Falha ao tentar confirmar usuário")]
        public async Task FalhaAoTentarConfirmarUsuario()
        {
            GivenQueOsDadosDoUsuarioSaoValidos();
            GivenSejaDiferenteDoUsuarioTechlanches();
            await GivenSejaExistente();
            GivenOUsuarioEstejaCadastrado();
            await WhenHouverFalhaAoConfirmarOUsuario();
            ThenRetornaOResultadoFalhaNaConfirmacaoDoUsuario();
        }

        private void GivenQueOsDadosDoUsuarioSaoValidos()
        {
            _usuario = _signUpFixture.GerarUsuario();
        }

        private void GivenQueOUsuarioETechLanches()
        {
            _usuario = _signUpFixture.GerarUsuarioTechLanches();
        }

        private void GivenSejaDiferenteDoUsuarioTechlanches()
        {
            _usuario.Should().NotBeEquivalentTo(_usuarioTechLanches);
        }

        private async Task GivenSejaExistente()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha(_signUpFixture.ObterMensagemFalha("usuario_cadastrado")));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        private void GivenSejaInexistente()
        {
            _cognitoService.SignUp(_usuario).Returns(Task.FromResult(Resultado.Ok()));
        }

        private void GivenOUsuarioEstejaCadastrado()
        {
            _cognitoService.SignUp(_usuario).Returns(Task.FromResult(Resultado.Ok()));
        }

        private void WhenCadastraOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Task.FromResult(Resultado.Ok()));
        }

        private async Task WhenConfirmaOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Ok());
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        private async Task WhenHouverFalhaAoCadastrarOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha(_signUpFixture.ObterMensagemFalha("usuario_nao_autorizado")));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        private async Task WhenORetornoDoCadastroDeUsuarioEDiferenteDoStatusCodeOK()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha(_signUpFixture.ObterMensagemFalha("status_code_diferente_ok")));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        private async Task WhenHouverFalhaAoConfirmarOUsuario()
        {
            _cognitoService.SignUp(_usuario).Returns(Resultado.Falha(_signUpFixture.ObterMensagemFalha("falha_ao_confirmar_usuario")));
            _resultado = await _cognitoService.SignUp(_usuario);
        }

        private void ThenRetornaOResultadoSucesso()
        {
            Assert.True(_resultado.Sucesso);
        }

        private void ThenRetornaOResultadoFalha()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signUpFixture.ObterMensagemFalha("usuario_nao_autorizado"), _resultado.Notificacoes.First().Mensagem);
        }

        private void ThenRetornaOResultadoFalhaPorStatusCode()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signUpFixture.ObterMensagemFalha("status_code_diferente_ok"), _resultado.Notificacoes.First().Mensagem);
        }

        private void ThenRetornaOResultadoFalhaDeUsuarioExistente()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signUpFixture.ObterMensagemFalha("usuario_cadastrado"), _resultado.Notificacoes.First().Mensagem);
        }

        private void ThenRetornaOResultadoFalhaNaConfirmacaoDoUsuario()
        {
            Assert.True(_resultado.Falhou);
            Assert.Equal(_signUpFixture.ObterMensagemFalha("falha_ao_confirmar_usuario"), _resultado.Notificacoes.First().Mensagem);
        }
    }
}
