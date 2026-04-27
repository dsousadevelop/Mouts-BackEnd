FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar arquivos de projeto individualmente para otimizar o cache
COPY ["src/Adapter/Drivers/Microservices/Ambev.DeveloperEvaluation.MicroServices/Ambev.DeveloperEvaluation.MicroServices.csproj", "src/Adapter/Drivers/Microservices/Ambev.DeveloperEvaluation.MicroServices/"]
COPY ["src/Ambev.DeveloperEvaluation.Domain/Ambev.DeveloperEvaluation.Domain.csproj", "src/Ambev.DeveloperEvaluation.Domain/"]

# Restore focado no MicroServices
RUN dotnet restore "./src/Adapter/Drivers/Microservices/Ambev.DeveloperEvaluation.MicroServices/Ambev.DeveloperEvaluation.MicroServices.csproj"

# Copiar o restante do código
COPY src/ src/

WORKDIR "/src/src/Adapter/Drivers/Microservices/Ambev.DeveloperEvaluation.MicroServices"
RUN dotnet build "./Ambev.DeveloperEvaluation.MicroServices.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ambev.DeveloperEvaluation.MicroServices.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER app
ENTRYPOINT ["dotnet", "Ambev.DeveloperEvaluation.MicroServices.dll"]
