using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrudProdutos.Application.Interfaces;
using CrudProdutos.Application.Services;
using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Data;
using CrudProdutos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

public class ProdutoServiceTestsDocker : IDisposable
{
    private readonly IProdutoService _produtoService;
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public ProdutoServiceTestsDocker()
    {
        // Configurar as opções do contexto do banco de dados a partir do appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .Options;

        // Criar um contexto de banco de dados com as opções configuradas
        var dbContext = new AppDbContext(_dbContextOptions);

        // Inicializar o serviço de produto com o contexto do banco de dados configurado
        _produtoService = new ProdutoService(new ProdutoRepository(dbContext));

        // Inicializar o banco de dados com dados de teste
        SeedTestData();
    }

    private void SeedTestData()
    {
        using (var dbContext = new AppDbContext(_dbContextOptions))
        {
            // Limpar os dados existentes
            dbContext.Produtos.RemoveRange(dbContext.Produtos);
            dbContext.SaveChanges();

            // Adicionar alguns produtos de teste ao banco de dados
            dbContext.Produtos.AddRange(new[]
            {
                new Produto { Id = 1, Nome = "Produto 1", Estoque = 10, Valor = 55.0m },
                new Produto { Id = 2, Nome = "Produto 2", Estoque = 20, Valor = 332.0m },
                new Produto { Id = 3, Nome = "Produto 3", Estoque = 15, Valor = 725.0m },
                new Produto { Id = 3, Nome = "Produto 4", Estoque = 15, Valor = 75.0m },
                new Produto { Id = 3, Nome = "Produto 5", Estoque = 15, Valor = 72.0m },
                new Produto { Id = 3, Nome = "Produto 6", Estoque = 15, Valor = 1725.0m },
            });

            dbContext.SaveChanges();
        }
    }

    [Fact]
    public async Task DeveListarProdutosCorretamente()
    {
        // Act
        var produtos = await _produtoService.ListarProdutosAsync();

        // Assert
        Assert.NotNull(produtos);
        Assert.Equal(3, produtos.Count());
    }

    [Fact]
    public async Task DeveObterProdutoPorIdCorretamente()
    {
        // Arrange
        var produtoExistenteId = 1;

        // Act
        var produto = await _produtoService.ObterProdutoPorIdAsync(produtoExistenteId);

        // Assert
        Assert.NotNull(produto);
        Assert.Equal(produtoExistenteId, produto.Id);
    }

    [Fact]
    public async Task DeveCriarProdutoCorretamente()
    {
        // Arrange
        var novoProduto = new Produto { Nome = "Novo Produto", Estoque = 10, Valor = 100.0m };

        // Act
        var novoProdutoId = await _produtoService.CriarProdutoAsync(novoProduto);

        // Assert
        Assert.NotEqual(0, novoProdutoId);
    }

    [Fact]
    public async Task DeveAtualizarProdutoCorretamente()
    {
        // Arrange
        var produtoExistenteId = 1;
        var produtoAtualizado = new Produto { Id = produtoExistenteId, Nome = "Produto Atualizado", Estoque = 30, Valor = 60.0m };

        // Act
        await _produtoService.AtualizarProdutoAsync(produtoExistenteId, produtoAtualizado);

        // Assert
        var produtoAtualizadoNovamente = await _produtoService.ObterProdutoPorIdAsync(produtoExistenteId);
        Assert.Equal(produtoAtualizado.Nome, produtoAtualizadoNovamente.Nome);
        Assert.Equal(produtoAtualizado.Estoque, produtoAtualizadoNovamente.Estoque);
        Assert.Equal(produtoAtualizado.Valor, produtoAtualizadoNovamente.Valor);
    }

    [Fact]
    public async Task DeveDeletarProdutoCorretamente()
    {
        // Arrange
        var produtoExistenteId = 1;

        // Act
        await _produtoService.DeletarProdutoAsync(produtoExistenteId);

        // Assert
        var produtoDeletado = await _produtoService.ObterProdutoPorIdAsync(produtoExistenteId);
        Assert.Null(produtoDeletado);
    }

    public void Dispose()
    {
        // Dispose do contexto do banco de dados
        using (var dbContext = new AppDbContext(_dbContextOptions))
        {
            dbContext.Database.EnsureDeleted();
        }
    }
}
