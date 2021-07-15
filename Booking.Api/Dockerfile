#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
#COPY docker-compose.dcproj /src/
COPY ["Booking.Api/Booking.Api.csproj", "Booking.Api/"]
COPY ["Booking.Core/Booking.Core.csproj", "Booking.Core/"]
COPY ["Tornado.Shared/Tornado.Shared.csproj", "Tornado.Shared/"]
RUN dotnet restore "Booking.Api/Booking.Api.csproj"
COPY . .
WORKDIR "/src/Booking.Api"
RUN dotnet build "Booking.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Booking.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booking.Api.dll"]