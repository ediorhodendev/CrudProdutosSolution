using CrudProdutos.Application.Interfaces;
using CrudProdutos.Application.Services;
using CrudProdutos.Domain.Models;
using CrudProdutos.Infrastructure.Data;
using CrudProdutos.Infrastructure.Interfaces;
using CrudProdutos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace CrudProdutosApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuração do Entity Framework para uso do SQL Server
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Injeção de dependência dos serviços e repositórios
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            // Configuração dos controllers da API
            services.AddControllers();

            // Adicione o serviço de migração do Entity Framework
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Registra a classe de inicialização do banco de dados como um serviço
            services.AddHostedService<DatabaseInitializer>();

            // Configuração do Swagger/OpenAPI
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Produtos", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configuração do Swagger em todos os ambientes
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Produtos v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Obtenha um escopo de serviços para acessar o contexto do banco de dados
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Obtenha o contexto do banco de dados
                    var context = services.GetRequiredService<AppDbContext>();

                    // Aplicar migrações pendentes (caso existam)
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Lide com exceções aqui, por exemplo, registre a exceção ou envie uma resposta de erro
                    // Você pode usar um sistema de registro como Serilog ou NLog para registrar exceções
                    // Ou retornar uma resposta de erro HTTP personalizada
                    // Exemplo: return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");

                    // Aqui, estamos apenas registrando a exceção no console
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
