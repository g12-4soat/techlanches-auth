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

        public async Task<Resultado> InativarDadosUsuarioPagamento(string cpf, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _httpClient.BaseAddress = new Uri($"http://localhost:5050/");

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/pagamentos/inativar/{cpf}", content);

                if (response.IsSuccessStatusCode == false)
                    return Resultado.Falha($"Erro durante chamada api de pedidos. Status Code:{response.StatusCode}. Response: {response}.");

                string resultStr = await response.Content.ReadAsStringAsync();

                return Resultado.Ok();
            }
            catch(Exception)
            {
                return Resultado.Falha("Falha ao ler arquivo string.");
            }
        }
    }
}
