#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Wallet.Api/Wallet.Api.csproj", "Wallet.Api/"]
COPY ["Tornado.Shared/Tornado.Shared.csproj", "Tornado.Shared/"]
COPY ["Wallet.Core/Wallet.Core.csproj", "Wallet.Core/"]
RUN dotnet restore "Wallet.Api/Wallet.Api.csproj"
COPY . .
WORKDIR "/src/Wallet.Api"
RUN dotnet build "Wallet.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wallet.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.Api.dll"]