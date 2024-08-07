﻿using System.Net.Http;
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
            _httpClient = httpClientFactory.CreateClient(Constants.NOME_API_PAGAMENTOS);
        }

        public PagamentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Resultado> InativarDadosUsuarioPagamento(string cpf, string token)
        {
            try
            {
                if (_httpClient == null)
                    return Resultado.Falha("HTTP Client não foi configurado corretamente");

                _httpClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"api/pagamentos/inativar/{cpf}");

                var contentStr = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode == false)
                    return Resultado.Falha($"Erro durante chamada api de pagamentos. Status Code:{response.StatusCode}. Response: {contentStr}.");

                string resultStr = await response.Content.ReadAsStringAsync();

                return Resultado.Ok();
            }
            catch(Exception ex)
            {
                return Resultado.Falha("Falha ao ler arquivo string da api/pagamentos/inativar => MESSAGE: " + ex.Message + "INNER EXCEPTION: " + ex.InnerException + "URI: " + _httpClient?.BaseAddress?.AbsolutePath);
            }
        }
    }
}
