using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudProdutos.Application.Interfaces;
using CrudProdutos.Application.Services;
using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Interfaces;
using Moq;
using Xunit;

public class ProdutoServiceTests
{
    [Fact]
    public async Task CriarProduto_DeveRetornarIdDoNovoProduto()
    {
        // Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock.Setup(repo => repo.CriarProdutoAsync(It.IsAny<Produto>()))
                            .ReturnsAsync(1);

        var produtoService = new ProdutoService(produtoRepositoryMock.Object);

        var novoProduto = new Produto { Nome = "Novo Produto", Estoque = 10, Valor = 100.0m };

        // Act
        var novoProdutoId = await produtoService.CriarProdutoAsync(novoProduto);

        // Assert
        Assert.Equal(1, novoProdutoId);
    }

    [Fact]
    public async Task ObterProdutoPorId_QuandoProdutoExiste_DeveRetornarProduto()
    {
        // Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock.Setup(repo => repo.ObterProdutoPorIdAsync(1))
                            .ReturnsAsync(new Produto { Id = 1, Nome = "Produto Existente", Estoque = 20, Valor = 50.0m });

        var produtoService = new ProdutoService(produtoRepositoryMock.Object);

        // Act
        var produto = await produtoService.ObterProdutoPorIdAsync(1);

        // Assert
        Assert.NotNull(produto);
        Assert.Equal(1, produto.Id);
        Assert.Equal("Produto Existente", produto.Nome);
        Assert.Equal(20, produto.Estoque);
        Assert.Equal(50.0m, produto.Valor);
    }

    [Fact]
    public async Task AtualizarProduto_QuandoProdutoExiste_DeveAtualizarProduto()
    {
        // Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock.Setup(repo => repo.AtualizarProdutoAsync(1, It.IsAny<Produto>()))
                            .Returns(Task.CompletedTask);

        var produtoService = new ProdutoService(produtoRepositoryMock.Object);

        var produtoAtualizado = new Produto { Id = 1, Nome = "Produto Atualizado", Estoque = 30, Valor = 60.0m };

        // Act
        await produtoService.AtualizarProdutoAsync(1, produtoAtualizado);

        // Assert - Verifique se o método de atualização do repositório foi chamado com os parâmetros corretos.
         produtoRepositoryMock.Verify(repo => repo.AtualizarProdutoAsync(1, produtoAtualizado), Times.Once);
    }

    [Fact]
    public async Task DeletarProduto_QuandoProdutoExiste_DeveDeletarProduto()
    {
        // Arrange
        var produtoRepositoryMock = new Mock<IProdutoRepository>();
        produtoRepositoryMock.Setup(repo => repo.DeletarProdutoAsync(1))
                            .Returns(Task.CompletedTask);

        var produtoService = new ProdutoService(produtoRepositoryMock.Object);

        // Act
        await produtoService.DeletarProdutoAsync(1);

        // Assert - Verifique se o método de exclusão do repositório foi chamado com o ID correto.
        produtoRepositoryMock.Verify(repo => repo.DeletarProdutoAsync(1), Times.Once);
    }

    
}
