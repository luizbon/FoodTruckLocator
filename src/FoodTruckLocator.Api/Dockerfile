#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
ARG USER=default
ARG HTTP_PORT=8080

WORKDIR /app
EXPOSE $HTTP_PORT

# create a new user
RUN adduser -D $USER

# execute the app under a non-root user
USER $USER

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["src/FoodTruckLocator.Api/FoodTruckLocator.Api.csproj", "FoodTruckLocator.Api/"]
RUN dotnet restore "FoodTruckLocator.Api/FoodTruckLocator.Api.csproj"
COPY . .
WORKDIR "/src/FoodTruckLocator.Api"
RUN dotnet build "FoodTruckLocator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodTruckLocator.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodTruckLocator.Api.dll"]