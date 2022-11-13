FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BankingSystem.Api/BankingSystem.Api.csproj", "BankingSystem.Api/"]
COPY ["BankingSystem.Application/BankingSystem.Application.csproj", "BankingSystem.Application/"]
COPY ["BankingSystem.Infrastructure/BankingSystem.Infrastructure.csproj", "BankingSystem.Infrastructure/"]
COPY ["BankingSystem.Domain/BankingSystem.Domain.csproj", "BankingSystem.Domain/"]
RUN dotnet restore "BankingSystem.Api/BankingSystem.Api.csproj"
COPY . .
WORKDIR "/src/BankingSystem.Api"
RUN dotnet build "BankingSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingSystem.Api.dll"]