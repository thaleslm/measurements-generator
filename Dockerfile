# Imagem base para SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base

COPY assets ./


WORKDIR /app

# Copia o arquivo .csproj e restaura as dependências
COPY measurement-generator.csproj ./
RUN dotnet restore "measurement-generator.csproj"

# Copia o restante do código para o contêiner
COPY . ./


# Defina a variável de ambiente para garantir que a ferramenta instalada seja encontrada
ENV PATH="${PATH}:/root/.dotnet/tools"

# Publica a aplicação
RUN dotnet publish "measurement-generator.csproj" -c Release -o /app/publish

# Imagem para execução do ASP.NET (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=base /app/publish .

# Expondo a porta 80 para a aplicação
EXPOSE 80

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "measurement-generator.dll"]
