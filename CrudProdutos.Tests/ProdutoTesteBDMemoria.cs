using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudProdutos.Application.Interfaces;
using CrudProdutos.Application.Services;
using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Data;
using CrudProdutos.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class ProdutoServiceTestsMemoria : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly AppDbContext _dbContext;

    public ProdutoServiceTestsMemoria()
    {
        // Configuração do DbContext em memória para os testes
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new AppDbContext(_dbContextOptions);
        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task CriarProduto_DeveRetornarIdDoNovoProduto()
    {
        // Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        var produtoService = new ProdutoService(produtoRepositoryMock.Object);

        var novoProduto = new Produto { Nome = "Novo Produto", Estoque = 10, Valor = 100.0m };

        // Act
        var novoProdutoId = await produtoService.CriarProdutoAsync(novoProduto);

        // Assert
        Assert.NotEqual(0, novoProdutoId); // O ID deve ser diferente de zero
    }

    [Fact]
    public async Task ObterProdutoPorId_QuandoProdutoExiste_DeveRetornarProduto()
    {
        // Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        var produtoService = new ProdutoService(produtoRepositoryMock.Object);

        var produtoExistente = new Produto { Nome = "Produto Existente", Estoque = 20, Valor = 50.0m };

        // Adicione o produto existente ao contexto em memória
        _dbContext.Produtos.Add(produtoExistente);
        _dbContext.SaveChanges();

        // Act
        var produto = await produtoService.ObterProdutoPorIdAsync(produtoExistente.Id);

        // Assert
        Assert.NotNull(produto);
        Assert.Equal(produtoExistente.Id, produto.Id);
        Assert.Equal(produtoExistente.Nome, produto.Nome);
        Assert.Equal(produtoExistente.Estoque, produto.Estoque);
        Assert.Equal(produtoExistente.Valor, produto.Valor);
    }

    [Fact]
    public async Task DeletarProduto_QuandoProdutoExiste_DeveDeletarProduto()
    {
        // Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        var produtoService = new ProdutoService(produtoRepositoryMock.Object);

        var produtoExistente = new Produto { Nome = "Produto Existente", Estoque = 20, Valor = 50.0m };

        // Adicione o produto existente ao contexto em memória
        _dbContext.Produtos.Add(produtoExistente);
        _dbContext.SaveChanges();

        // Act
        await produtoService.DeletarProdutoAsync(produtoExistente.Id);

        // Assert
        var produtoDeletado = await _dbContext.Produtos.FindAsync(produtoExistente.Id);
        Assert.Null(produtoDeletado); // O produto deve ser nulo após a exclusão
    }

    public void Dispose()
    {
        // Limpe o contexto em memória após os testes
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
