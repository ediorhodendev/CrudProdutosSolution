using CrudProdutos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrudProdutos.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de mapeamento para a entidade Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                // Define o nome da tabela
                entity.ToTable("Produtos");

                // Define a chave primária
                entity.HasKey(p => p.Id);

                // Configurações de colunas
                entity.Property(p => p.Id).HasColumnName("Id");
                entity.Property(p => p.Nome).HasColumnName("Nome").HasMaxLength(100).IsRequired();
                entity.Property(p => p.Estoque).HasColumnName("Estoque").IsRequired();
                entity.Property(p => p.Valor).HasColumnName("Valor").HasColumnType("decimal(18, 2)").IsRequired();

                // configuração de índice
                entity.HasIndex(p => p.Nome).IsUnique();

                
            });
        }
    }
}
