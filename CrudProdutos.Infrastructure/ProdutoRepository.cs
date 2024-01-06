using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Data;
using CrudProdutos.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrudProdutos.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _dbContext;

        public ProdutoRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAsync()
        {
            return await _dbContext.Produtos.ToListAsync();
        }

        public async Task<Produto> ObterProdutoPorIdAsync(int id)
        {
            return await _dbContext.Produtos.FindAsync(id);
        }

        public async Task<IEnumerable<Produto>> BuscarProdutosPorNomeAsync(string nome)
        {
            return await _dbContext.Produtos
                .Where(p => p.Nome.Contains(nome))
                .ToListAsync();
        }

        public async Task<int> CriarProdutoAsync(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _dbContext.Produtos.Add(produto);
            await _dbContext.SaveChangesAsync();
            return produto.Id;
        }

        public async Task AtualizarProdutoAsync(int v, Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            if (!_dbContext.Produtos.Local.Contains(produto))
            {
                _dbContext.Entry(produto).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletarProdutoAsync(int id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);

            if (produto == null)
            {
                throw new KeyNotFoundException("Produto não encontrado.");
            }

            _dbContext.Produtos.Remove(produto);
            await _dbContext.SaveChangesAsync();
        }
    }
}
