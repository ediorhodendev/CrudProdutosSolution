using System.Collections.Generic;
using System.Threading.Tasks;
using CrudProdutos.Application.Interfaces;
using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

using CrudProdutos.Infrastructure.Repositories;

using System.Collections.Generic;
using System.Linq;


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

            await _produtoRepository.AtualizarProdutoAsync(1, produtoExistente);
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


       

        public async Task<IEnumerable<Produto>> ListarProdutosOrdenadosAsync(OrderByField campo, bool crescente)
        {
            var produtosQuery = await _produtoRepository.ListarProdutosAsync();

            switch (campo)
            {
                case OrderByField.Nome:
                    produtosQuery = crescente
                        ? produtosQuery.OrderBy(p => p.Nome)
                        : produtosQuery.OrderByDescending(p => p.Nome);
                    break;

                case OrderByField.Estoque:
                    produtosQuery = crescente
                        ? produtosQuery.OrderBy(p => p.Estoque)
                        : produtosQuery.OrderByDescending(p => p.Estoque);
                    break;

                case OrderByField.Valor:
                    produtosQuery = crescente
                        ? produtosQuery.OrderBy(p => p.Valor)
                        : produtosQuery.OrderByDescending(p => p.Valor);
                    break;

                default:
                    // Se o campo de ordenação for inválido, não realizar nenhuma ordenação
                    break;
            }

            var produtosList = produtosQuery.ToList();
            return produtosList.AsEnumerable();
        }




    }
}
