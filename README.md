# Projeto CRUD de Produtos

Este é um projeto de exemplo que demonstra como criar uma API de CRUD de produtos usando .NET 6, Entity Framework Core, Docker e Swagger/OpenAPI.

## Estrutura da Solution

A Solution consiste nos seguintes projetos:

- **CrudProdutosApi**: O projeto principal que contém a API da aplicação.
- **CrudProdutos.Application**: Camada de aplicação que lida com a lógica de negócios.
- **CrudProdutos.Domain**: Camada de domínio que define os modelos de dados da aplicação.
- **CrudProdutos.Infrastructure**: Camada de infraestrutura que lida com o acesso ao banco de dados e implementações concretas.
- **CrudProdutos.Tests**: Projetos de testes unitários para testar a aplicação.

## Funcionalidades

- Criação de um novo produto.
- Atualização de um produto existente.
- Exclusão de um produto.
- Listagem de todos os produtos.
- Consulta de um produto específico por ID.
- Ordenação de produtos por diferentes campos.
- Busca de produtos por nome.

## Requisitos

- .NET 6 ou superior.
- Entity Framework Core.
- Docker (para o banco de dados SQL Server).
- Swagger/OpenAPI para documentação da API.
- Testes unitários para garantir a qualidade do código.
-  Testes de integração

## Configuração

Certifique-se de configurar a string de conexão com o banco de dados SQL Server no arquivo `appsettings.json` do projeto `CrudProdutosApi`.
 "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1450;Database=crudprodutos;User Id=sa;Password=Pedro@123;"
  },
Se necessário altera as portas. 
### Utilizando Docker e SQL Server

Este projeto utiliza Docker para executar um contêiner do SQL Server. Siga as instruções abaixo:

1. **Instale o Docker**: Se você ainda não tiver o Docker instalado, faça o download e instale-o a partir do [site oficial do Docker](https://www.docker.com/get-started).

2. **Execute o Contêiner do SQL Server**:

   Execute o seguinte comando no terminal para baixar e iniciar um contêiner do SQL Server: na raiz do projeto execute o comando para criar as imagens no docker
   
   docker-compose up
   Obs: verifique se o arquivo docker-compose.yml está na raiz do projeto.

   Conteúdo do arquivo
version: '3.9'
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2019-latest
    container_name: sqlserver-container
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "Pedro@123"
      MSSQL_DBNAME: "crudprodutos"
    ports:
      - "1450:1433"
    networks:
      - crudprodutos-net
    restart: unless-stopped

networks:
  crudprodutos-net:
    driver: bridge


   

   

