using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Service
{
    public interface IPagamentoService
    {
        Task<Resultado> InativarDadosUsuarioPagamento(string cpf, string token);
    }

    public class PagamentoService : IPagamentoService

    {
        private readonly HttpClient _httpClient;

        public PagamentoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("pagamentos");
        }

        public async Task<Resultado> InativarDadosUsuarioPagamento(string cpf, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/pagamentos/inativar-dados/{cpf}", content);

                if (response.IsSuccessStatusCode == false)
                    return Resultado.Falha($"Erro durante chamada api de pedidos. Status Code:{response.StatusCode}. Response: {response}.");

                string resultStr = await response.Content.ReadAsStringAsync();

                return Resultado.Ok();
            }
            catch(Exception ex)
            {
                return Resultado.Falha("Falha ao ler arquivo string.");
            }
        }
    }
}
