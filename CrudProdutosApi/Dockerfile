#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CrudProdutosApi/CrudProdutosApi.csproj", "CrudProdutosApi/"]
COPY ["CrudProdutos.Application/CrudProdutos.Application.csproj", "CrudProdutos.Application/"]
COPY ["CrudProdutos.Domain/CrudProdutos.Domain.csproj", "CrudProdutos.Domain/"]
COPY ["CrudProdutos.Infrastructure/CrudProdutos.Infrastructure.csproj", "CrudProdutos.Infrastructure/"]
RUN dotnet restore "CrudProdutosApi/CrudProdutosApi.csproj"
COPY . .
WORKDIR "/src/CrudProdutosApi"
RUN dotnet build "CrudProdutosApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CrudProdutosApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CrudProdutosApi.dll"]