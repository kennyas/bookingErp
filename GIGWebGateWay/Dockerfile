FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["GIGWebGateWay/GIGWebGateWay.csproj", "GIGWebGateWay/"]

RUN dotnet restore "GIGWebGateWay/GIGWebGateWay.csproj"
COPY . .
WORKDIR "/src/GIGWebGateWay"
RUN dotnet build "GIGWebGateWay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GIGWebGateWay.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GIGWebGateWay.dll"]