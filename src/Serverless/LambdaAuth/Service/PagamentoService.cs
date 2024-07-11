using System.Net.Http.Headers;
using System.Text;
using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Service
{
    public class PagamentoService : IPagamentoService
    {
        private readonly HttpClient _httpClient;

        public PagamentoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.PAGAMENTOS);
        }

        public PagamentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Resultado> InativarDadosUsuarioPagamento(string cpf, string token)
        {
            try
            {
                //_httpClient.BaseAddress = new Uri($"https://bs4rjn7ju9.execute-api.us-east-1.amazonaws.com/dev/pagamento/");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                var response = await _httpClient.DeleteAsync($"api/pagamentos/inativar/{cpf}");

                if (response.IsSuccessStatusCode == false)
                    return Resultado.Falha($"Erro durante chamada api de pagamentos. Status Code:{response.StatusCode}. Response: {response.Content}.");

                string resultStr = await response.Content.ReadAsStringAsync();

                return Resultado.Ok();
            }
            catch(Exception ex)
            {
                return Resultado.Falha("Falha ao ler arquivo string da api/pagamentos/inativar => MESSAGE: " + ex.Message + "INNER EXCEPTION: " + ex.InnerException + "URI: " + _httpClient.BaseAddress);
            }
        }
    }
}
