# Imagem base para SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base

COPY assets ./


WORKDIR /app

# Copia o arquivo .csproj e restaura as depend�ncias
COPY measurement-generator.csproj ./
RUN dotnet restore "measurement-generator.csproj"

# Copia o restante do c�digo para o cont�iner
COPY . ./


# Defina a vari�vel de ambiente para garantir que a ferramenta instalada seja encontrada
ENV PATH="${PATH}:/root/.dotnet/tools"

# Publica a aplica��o
RUN dotnet publish "measurement-generator.csproj" -c Release -o /app/publish

# Imagem para execu��o do ASP.NET (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=base /app/publish .

# Expondo a porta 80 para a aplica��o
EXPOSE 80

# Comando para rodar a aplica��o
ENTRYPOINT ["dotnet", "measurement-generator.dll"]
