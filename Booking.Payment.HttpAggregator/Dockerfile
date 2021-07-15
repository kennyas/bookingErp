FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Booking.Payment.HttpAggregator.core/Booking.Payment.HttpAggregator.core.csproj", "Booking.Payment.HttpAggregator.core/"]
COPY ["Booking.Payment.HttpAggregator/Booking.Payment.HttpAggregator.csproj", "Booking.Payment.HttpAggregator/"]
COPY ["Tornado.Shared/Tornado.Shared.csproj", "Tornado.Shared/"]

RUN dotnet restore "Booking.Payment.HttpAggregator/Booking.Payment.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/Booking.Payment.HttpAggregator"
RUN dotnet build "Booking.Payment.HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Booking.Payment.HttpAggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booking.Payment.HttpAggregator.dll"]