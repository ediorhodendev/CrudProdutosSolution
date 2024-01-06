using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Verifique se já existem produtos no banco de dados
            if (!dbContext.Produtos.Any())
            {
                // Se não existirem produtos, adicione 5 produtos iniciais
                dbContext.Produtos.AddRange(new[]
                {
                    new Produto { Nome = "Produto 1", Estoque = 10, Valor = 50.0m },
                    new Produto { Nome = "Produto 2", Estoque = 20, Valor = 60.0m },
                    new Produto { Nome = "Produto 3", Estoque = 15, Valor = 40.0m },
                    new Produto { Nome = "Produto 4", Estoque = 30, Valor = 70.0m },
                    new Produto { Nome = "Produto 5", Estoque = 25, Valor = 55.0m }
                });

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
