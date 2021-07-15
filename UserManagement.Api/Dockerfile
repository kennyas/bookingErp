#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["UserManagement.Api/UserManagement.Api.csproj", "UserManagement.Api/"]
COPY ["UserManagement.Core/UserManagement.Core.csproj", "UserManagement.Core/"]
COPY ["Tornado.Shared/Tornado.Shared.csproj", "Tornado.Shared/"]
RUN dotnet restore "UserManagement.Api/UserManagement.Api.csproj"
COPY . .
WORKDIR "/src/UserManagement.Api"
RUN dotnet build "UserManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.Api.dll"]