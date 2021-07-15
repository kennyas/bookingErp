#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Notify.Api/Notify.Api.csproj", "Notify.Api/"]
COPY ["Notify.Core/Notify.Core.csproj", "Notify.Core/"]
COPY ["Tornado.Shared/Tornado.Shared.csproj", "Tornado.Shared/"]
RUN dotnet restore "Notify.Api/Notify.Api.csproj"
COPY . .
WORKDIR "/src/Notify.Api"
RUN dotnet build "Notify.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notify.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notify.Api.dll"]