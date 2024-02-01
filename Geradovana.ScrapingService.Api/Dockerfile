#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Geradovana.ScrapingService.Api/Geradovana.ScrapingService.Api.csproj", "Geradovana.ScrapingService.Api/"]
COPY ["Geradovana.ScrapingService.Application/Geradovana.ScrapingService.Application.csproj", "Geradovana.ScrapingService.Application/"]
COPY ["Geradovana.ScrapingService/Geradovana.ScrapingService.Domain.csproj", "Geradovana.ScrapingService/"]
COPY ["Geradovana.ScrapingService.Contracts/Geradovana.ScrapingService.Contracts.csproj", "Geradovana.ScrapingService.Contracts/"]
COPY ["Geradovana.ScrapingService.Infrastructure/Geradovana.ScrapingService.Infrastructure.csproj", "Geradovana.ScrapingService.Infrastructure/"]
RUN dotnet restore "./Geradovana.ScrapingService.Api/./Geradovana.ScrapingService.Api.csproj"
COPY . .
WORKDIR "/src/Geradovana.ScrapingService.Api"
RUN dotnet build "./Geradovana.ScrapingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Geradovana.ScrapingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Geradovana.ScrapingService.Api.dll"]