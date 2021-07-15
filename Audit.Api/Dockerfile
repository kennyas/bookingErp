FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Audit.Api/Audit.Api.csproj", "Audit.Api/"]
COPY ["Audit.Core/Audit.Core.csproj", "Audit.Core/"]
COPY ["Tornado.Shared/Tornado.Shared.csproj", "Tornado.Shared/"]

RUN dotnet restore "Audit.Api/Audit.Api.csproj"
COPY . .
WORKDIR "/src/Audit.Api"
RUN dotnet build "Audit.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Audit.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Audit.Api.dll"]