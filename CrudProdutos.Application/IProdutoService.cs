using System.Collections.Generic;
using System.Threading.Tasks;
using CrudProdutos.Domain.Models;

namespace CrudProdutos.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ListarProdutosAsync();
        Task<Produto> ObterProdutoPorIdAsync(int id);
        Task<int> CriarProdutoAsync(Produto produto);
        Task AtualizarProdutoAsync(int id, Produto produto);
        Task<IEnumerable<Produto>> BuscarProdutosPorNomeAsync(string nome); // Novo método
        Task DeletarProdutoAsync(int id);
    }
}
