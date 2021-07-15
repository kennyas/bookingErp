FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Report.Api/Report.Api.csproj", "Report.Api/"]
COPY ["Report.Core/Report.Core.csproj", "Report.Core/"]
COPY ["Tornado.Shared/Tornado.Shared.csproj", "Tornado.Shared/"]

RUN dotnet restore "Report.Api/Report.Api.csproj"
COPY . .
WORKDIR "/src/Report.Api"
RUN dotnet build "Report.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Report.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Report.Api.dll"]