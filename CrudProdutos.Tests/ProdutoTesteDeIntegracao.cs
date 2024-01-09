using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CrudProdutos.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CrudProdutos.Tests
{
    public class ProdutoControllerIntegrationTests : IClassFixture<WebApplicationFactory<CrudProdutosApi.Startup>>
    {
        private readonly HttpClient _client;

        public ProdutoControllerIntegrationTests(WebApplicationFactory<CrudProdutosApi.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task DeveCriarBuscarAtualizarExcluirProdutoComSucesso()
        {
            // Criar um novo produto
            var novoProduto = new Produto
            {
                Nome = "Novo Produto",
                Estoque = 10,
                Valor = 100.0m
            };

            var createResponse = await _client.PostAsync("/api/Produtos", SerializeToContent(novoProduto));
            createResponse.EnsureSuccessStatusCode();
            var produtoCriado = await DeserializeResponse<Produto>(createResponse);

            // Buscar o produto pelo ID
            var getByIdResponse = await _client.GetAsync($"/api/Produtos/{produtoCriado.Id}");
            getByIdResponse.EnsureSuccessStatusCode();
            var produtoBuscado = await DeserializeResponse<Produto>(getByIdResponse);

            // Atualizar o produto
            var produtoAtualizado = new Produto
            {
                Id = produtoBuscado.Id,
                Nome = "Produto Atualizado",
                Estoque = 20,
                Valor = 150.0m
            };

            var updateResponse = await _client.PutAsync($"/api/Produtos/{produtoAtualizado.Id}", SerializeToContent(produtoAtualizado));
            updateResponse.EnsureSuccessStatusCode();

            // Buscar o produto atualizado pelo ID
            var updatedResponse = await _client.GetAsync($"/api/Produtos/{produtoAtualizado.Id}");
            updatedResponse.EnsureSuccessStatusCode();
            var produtoAtualizadoBuscado = await DeserializeResponse<Produto>(updatedResponse);

            // Excluir o produto
            var deleteResponse = await _client.DeleteAsync($"/api/Produtos/{produtoAtualizado.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            // Verificar se o produto foi excluído
            var notFoundResponse = await _client.GetAsync($"/api/Produtos/{produtoAtualizado.Id}");
            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);
        }

        private StringContent SerializeToContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
