using System.Collections.Generic;
using System.Threading.Tasks;
using CrudProdutos.Application.Interfaces;
using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Interfaces;
using OpenQA.Selenium;

namespace CrudProdutos.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAsync()
        {
            return await _produtoRepository.ListarProdutosAsync();
            
        }

        public async Task<Produto> ObterProdutoPorIdAsync(int id)
        {
            return await _produtoRepository.ObterProdutoPorIdAsync(id);
        }

        public async Task<int> CriarProdutoAsync(Produto produto)
        {
            if (produto.Valor < 0)
            {
                throw new ArgumentException("O valor do produto não pode ser negativo.");
            }

            return await _produtoRepository.CriarProdutoAsync(produto);
        }

        public async Task AtualizarProdutoAsync(int id, Produto produto)
        {
            var produtoExistente = await _produtoRepository.ObterProdutoPorIdAsync(id);

            if (produtoExistente == null)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            if (produto.Valor < 0)
            {
                throw new ArgumentException("O valor do produto não pode ser negativo.");
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Estoque = produto.Estoque;
            produtoExistente.Valor = produto.Valor;

            await _produtoRepository.AtualizarProdutoAsync(produtoExistente);
        }

        public async Task<IEnumerable<Produto>> BuscarProdutosPorNomeAsync(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do produto não pode ser vazio ou nulo.");
        }

        var produtos = await _produtoRepository.BuscarProdutosPorNomeAsync(nome);
        return produtos;
    }

        public async Task DeletarProdutoAsync(int id)
        {
            var produtoExistente = await _produtoRepository.ObterProdutoPorIdAsync(id);

            if (produtoExistente == null)
            {
                throw new NotFoundException("Produto não encontrado.");
            }

            await _produtoRepository.DeletarProdutoAsync(produtoExistente.Id);
        }
    }
}
