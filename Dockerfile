# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Tienda.Microservicios.Autor.Api.csproj .
RUN dotnet restore Tienda.Microservicios.Autor.Api.csproj
COPY . .
RUN dotnet build Tienda.Microservicios.Autor.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Tienda.Microservicios.Autor.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tienda.Microservicios.Autor.Api.dll"]
