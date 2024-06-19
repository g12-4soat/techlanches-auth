using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TechLanchesLambda.AWS.Options;
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
                                                 [FromServices] IOptions<AWSOptions> awsOptions)
    {
        try
        {
            context.Logger.LogInformation("Handling the 'LambdaInativacao' Request");

            ArgumentNullException.ThrowIfNull(awsOptions);

            var usuario = ObterUsuario(request, awsOptions.Value, ehCadastro: false);
            var usuarioEhPadrao = usuario.Cpf.Equals(awsOptions.Value.UserTechLanches);

            if (usuarioEhPadrao)
                return Response.BadRequest("N�o � poss�vel realizar a inativa��o deste usu�rio.");

            var resultadoLogin = await cognitoService.SignIn(usuario.Cpf);

            if (!resultadoLogin.Sucesso)
                return Response.BadRequest(resultadoLogin.Notificacoes);

            var pagamento = await pagamentoService.InativarDadosUsuarioPagamento(usuario.Cpf, resultadoLogin.Value.AccessToken);

            if (pagamento.Falhou)
                return Response.BadRequest(pagamento.Notificacoes);

            //se resultado positivo consumir endpoint de pagamento para inativacao dos dados
            var usuarioInativado = await cognitoService.InativarUsuario(usuario.Cpf);

            return Response.Ok("ok");
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

        if (!CpfFoiInformado(usuario) && !ehCadastro)
            return new UsuarioDto(awsOptions.UserTechLanches, awsOptions.EmailDefault, awsOptions.UserTechLanches);

        string cpf = usuario.Cpf ?? string.Empty;
        string cpfLimpo = ValidatorCPF.LimparCpf(cpf);
        string email = usuario.Email ?? string.Empty;
        string nome = usuario.Nome ?? string.Empty;
        string endereco = usuario.Endereco ?? string.Empty; // verificar se vai criar classe para validar endere�o com que ser� tratado
        string telefone = usuario.Telefone ?? string.Empty; 
        var user = new UsuarioDto(string.IsNullOrEmpty(cpfLimpo) ? cpf : cpfLimpo, email, nome, endereco, telefone);
        return user;
    }

    private bool CpfFoiInformado(UsuarioDto usuario)
    {
        return !string.IsNullOrEmpty(usuario.Cpf) && !string.IsNullOrWhiteSpace(usuario.Cpf);
    }
}
