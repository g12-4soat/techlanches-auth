using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Net;
using TechLanchesLambda.DTOs;

namespace TechLanchesLambda.Utils;

public static class Response
{
    public static APIGatewayProxyResponse BadRequest(List<NotificacaoDto> notificacoes)
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Body = JsonConvert.SerializeObject(notificacoes),
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/jsonn"  },
                { "X-Content-Type-Options", "nosniff" },
                { "Strict-Transport-Security", "max-age=1; includeSubDomains; preload" }
            }
        };

        return response;
    }
    

    public static APIGatewayProxyResponse BadRequest(string mensagem)
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Body = JsonConvert.SerializeObject(new NotificacaoDto(mensagem)),
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/jsonn"  },
                { "X-Content-Type-Options", "nosniff" },
                { "Strict-Transport-Security", "max-age=1; includeSubDomains; preload" }
            }
        };

        return response;
    }

    public static APIGatewayProxyResponse Ok(object body)
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JsonConvert.SerializeObject(body),
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/jsonn"  },
                { "X-Content-Type-Options", "nosniff" },
                { "Strict-Transport-Security", "max-age=1; includeSubDomains; preload" }
            }
        };

        return response;
    }
}