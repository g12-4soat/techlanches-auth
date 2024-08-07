using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TechLanchesLambda.AWS.Options;
using TechLanchesLambda.Controller;
using TechLanchesLambda.DTOs;
using TechLanchesLambda.Service;
using TechLanchesLambda.Utils;
using TechLanchesLambda.Validations;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TechLanchesLambda;

public class Functions
{
    public Functions()
    {
    }

    [LambdaFunction(Policies = "AWSLambdaBasicExecutionRole", MemorySize = 512, Timeout = 30)]
    [RestApi(LambdaHttpMethod.Post, "/auth")]
    public async Task<APIGatewayProxyResponse> LambdaAuth(APIGatewayProxyRequest request,
                                                  ILambdaContext context,
                                                  [FromServices] ICognitoService cognitoService,
                                                  [FromServices] IOptions<AWSOptions> awsOptions)
    {
        try
        {
            context.Logger.LogInformation("Handling the 'LambdaAuth' Request");
            ArgumentNullException.ThrowIfNull(awsOptions);

            var usuario = ObterUsuario(request, awsOptions.Value, ehCadastro: false);
            var usuarioEhPadrao = usuario.Cpf.Equals(awsOptions.Value.UserTechLanches);
            if (usuarioEhPadrao)
            {
                var resultadoCadastroUsuario = await cognitoService.SignUp(usuario);
                if (!resultadoCadastroUsuario.Sucesso)
                {
                    return Response.BadRequest(resultadoCadastroUsuario.Notificacoes);
                }
            }

            var resultadoLogin = await cognitoService.SignIn(usuario.Cpf);
            if (!resultadoLogin.Sucesso)
            {
                return Response.BadRequest(resultadoLogin.Notificacoes);
            }

            var tokenResult = resultadoLogin.Value;
            return !string.IsNullOrEmpty(tokenResult.AccessToken) ? Response.Ok(tokenResult) : Response.BadRequest("N�o possui token"); 
        }
        catch (Exception ex)
        {
            Console.WriteLine("Auth Lambda response error: " + ex.Message);
            throw new Exception(ex.Message);
        }
    }

    [LambdaFunction(Policies = "AWSLambdaBasicExecutionRole", MemorySize = 512, Timeout = 30)]
    [RestApi(LambdaHttpMethod.Post, "/cadastro")]
    public async Task<APIGatewayProxyResponse> LambdaCadastro(APIGatewayProxyRequest request,
                                                  ILambdaContext context,
                                                  [FromServices] ICognitoService cognitoService,
                                                  [FromServices] IOptions<AWSOptions> awsOptions)
    {
        try
        {
            context.Logger.LogInformation("Handling the 'LambdaCadastro' Request");

            ArgumentNullException.ThrowIfNull(awsOptions);

            var usuario = ObterUsuario(request, awsOptions.Value, ehCadastro: true);

            var resultadoValidacaoUsuario = new UsuarioCadastroValidation().Validate(usuario);
            if(!resultadoValidacaoUsuario.IsValid)
            {
                return Response.BadRequest(resultadoValidacaoUsuario.Errors.Select(x => new NotificacaoDto(x.ErrorMessage)).ToList());
            }
            
            var resultadoCadastroUsuario = await cognitoService.SignUp(usuario);
            if (!resultadoCadastroUsuario.Sucesso)
            {
                return Response.BadRequest(resultadoCadastroUsuario.Notificacoes);
            }

            var resultadoLogin = await cognitoService.SignIn(usuario.Cpf);
            if (!resultadoLogin.Sucesso)
            {
                return Response.BadRequest(resultadoLogin.Notificacoes);
            }

            var tokenResult = resultadoLogin.Value;
            return !string.IsNullOrEmpty(tokenResult.AccessToken) ? Response.Ok(tokenResult) : Response.BadRequest("N�o possui token");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Cadastro Lambda response error: " + ex.Message);
            throw new Exception(ex.Message);
        }
    }

    [LambdaFunction(Policies = "AWSLambdaBasicExecutionRole", MemorySize = 512, Timeout = 30)]
    [RestApi(LambdaHttpMethod.Post, "/inativacao")]
    public async Task<APIGatewayProxyResponse> LambdaInativacao(APIGatewayProxyRequest request,
                                                 ILambdaContext context,
                                                 [FromServices] ICognitoService cognitoService,
                                                 [FromServices] IPagamentoService pagamentoService,
                                                 [FromServices] IPedidoService pedidoService,
                                                 [FromServices] IUsuarioInativoController usuarioInativoController,
                                                 [FromServices] IOptions<AWSOptions> awsOptions)
    {
        try
        {
            context.Logger.LogInformation("Handling the 'LambdaInativacao' Request");

            ArgumentNullException.ThrowIfNull(awsOptions);

            var usuario = ObterUsuarioInativacao(request, awsOptions.Value);
            var usuarioEhPadrao = usuario.Cpf.Equals(awsOptions.Value.UserTechLanches);

            if (usuarioEhPadrao)
                return Response.BadRequest("N�o � poss�vel realizar a inativa��o deste usu�rio.");

            var resultadoLogin = await cognitoService.SignIn(usuario.Cpf);

            if (!resultadoLogin.Sucesso)
                return Response.BadRequest(resultadoLogin.Notificacoes);

            var cadastrarInativacaoUsuario = await usuarioInativoController.Cadastrar(usuario);

            if (!cadastrarInativacaoUsuario.Sucesso)
                return Response.BadRequest(cadastrarInativacaoUsuario.Notificacoes);

            var pagamentosUsuarioInativo = await pagamentoService.InativarDadosUsuarioPagamento(usuario.Cpf, resultadoLogin.Value.AccessToken);

            if (pagamentosUsuarioInativo.Falhou)
                return Response.BadRequest(pagamentosUsuarioInativo.Notificacoes);

            var pedidosUsuarioInativo = await pedidoService.InativarDadosUsuarioPedido(usuario.Cpf, resultadoLogin.Value.AccessToken);

            if (pedidosUsuarioInativo.Falhou)
                return Response.BadRequest(pedidosUsuarioInativo.Notificacoes);

            var usuarioInativado = await cognitoService.InativarUsuario(usuario.Cpf);

            return usuarioInativado.Sucesso ? 
                   Response.Ok(usuarioInativado.Value) : 
                   Response.BadRequest(usuarioInativado.Notificacoes);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Inativa��o Lambda response error: " + ex.Message);
            throw new Exception(ex.Message);
        }
    }

    private UsuarioDto ObterUsuario(APIGatewayProxyRequest request, AWSOptions awsOptions, bool ehCadastro)
    {
        var usuario = JsonConvert.DeserializeObject<UsuarioDto>(request.Body) ?? new UsuarioDto();
        ArgumentNullException.ThrowIfNull(usuario);

        if (!CpfFoiInformado(usuario.Cpf) && !ehCadastro)
            return new UsuarioDto(awsOptions.UserTechLanches, awsOptions.EmailDefault, awsOptions.UserTechLanches);

        string cpf = usuario.Cpf ?? string.Empty;
        string cpfLimpo = ValidatorCPF.LimparCpf(cpf);
        string email = usuario.Email ?? string.Empty;
        string nome = usuario.Nome ?? string.Empty;
        var user = new UsuarioDto(string.IsNullOrEmpty(cpfLimpo) ? cpf : cpfLimpo, email, nome);
        return user;
    }

    private UsuarioInativacaoDto ObterUsuarioInativacao(APIGatewayProxyRequest request, AWSOptions awsOptions)
    {
        var usuario = JsonConvert.DeserializeObject<UsuarioInativacaoDto>(request.Body) ?? new UsuarioInativacaoDto();
        ArgumentNullException.ThrowIfNull(usuario);

        if (!CpfFoiInformado(usuario.Cpf))
            return new UsuarioInativacaoDto(awsOptions.UserTechLanches, awsOptions.EmailDefault, awsOptions.UserTechLanches);

        string cpf = usuario.Cpf ?? string.Empty;
        string cpfLimpo = ValidatorCPF.LimparCpf(cpf);
        string email = usuario.Email ?? string.Empty;
        string nome = usuario.Nome ?? string.Empty;
        string endereco = usuario.Endereco ?? string.Empty;
        string telefone = usuario.Telefone ?? string.Empty;
        var user = new UsuarioInativacaoDto(string.IsNullOrEmpty(cpfLimpo) ? cpf : cpfLimpo, nome, email, endereco, telefone);
        return user;
    }

    private bool CpfFoiInformado(string cpf)
    {
        return !string.IsNullOrEmpty(cpf) && !string.IsNullOrWhiteSpace(cpf);
    }
}