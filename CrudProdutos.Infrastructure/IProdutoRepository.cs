
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudProdutos.Domain.Models;

namespace CrudProdutos.Infrastructure.Interfaces
{
    public interface IProdutoRepository
    {
        /// <summary>
        /// Obtém uma lista de todos os produtos de forma assíncrona.
        /// </summary>
        /// <returns>Uma coleção de produtos.</returns>
        Task<IEnumerable<Produto>> ListarProdutosAsync();

        /// <summary>
        /// Obtém um produto por ID de forma assíncrona.
        /// </summary>
        /// <param name="id">O ID do produto a ser obtido.</param>
        /// <returns>O produto correspondente ao ID especificado.</returns>
        Task<Produto> ObterProdutoPorIdAsync(int id);

        /// <summary>
        /// Cria um novo produto de forma assíncrona.
        /// </summary>
        /// <param name="produto">O produto a ser criado.</param>
        /// <returns>O ID do produto criado.</returns>
        Task<int> CriarProdutoAsync(Produto produto);

        /// <summary>
        /// Atualiza um produto existente de forma assíncrona.
        /// </summary>
        /// <param name="produto">O produto atualizado.</param>
        Task AtualizarProdutoAsync(int v, Produto produto);

        /// <summary>
        /// Atualiza um produto existente de forma assíncrona.
        /// </summary>
        /// <param name="nome">O produto atualizado.</param>
        Task<IEnumerable<Produto>> BuscarProdutosPorNomeAsync(string nome);

        /// <summary>
        /// Exclui um produto de forma assíncrona.
        /// </summary>
        /// <param name="produto">O produto a ser excluído.</param>
        Task DeletarProdutoAsync(int id);
    }
}



