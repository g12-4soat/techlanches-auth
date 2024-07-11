﻿using System.Net.Http.Headers;
using System.Text;
using TechLanchesLambda.Utils;

namespace TechLanchesLambda.Service
{
    public class PedidoService : IPedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.PEDIDOS);
        }

        public PedidoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Resultado> InativarDadosUsuarioPedido(string cpf, string token)
        {
            try
            {
                //_httpClient.BaseAddress = new Uri($"https://bs4rjn7ju9.execute-api.us-east-1.amazonaws.com/dev/pedido/");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/pedidos/inativar/{cpf}", content);

                if (response.IsSuccessStatusCode == false)
                    return Resultado.Falha($"Erro durante chamada api de pedidos. Status Code:{response.StatusCode}. Response: {response}.");

                string resultStr = await response.Content.ReadAsStringAsync();

                return Resultado.Ok();
            }
            catch (Exception ex)
            {
                return Resultado.Falha("Falha ao ler arquivo string da api/pedidos/inativar => MESSAGE: " + ex.Message + "INNER EXCEPTION: " + ex.InnerException + "URI: " + _httpClient.BaseAddress);
            }
        }
    }
}
