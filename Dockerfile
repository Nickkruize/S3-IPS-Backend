# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish -c Release -o out

# Build runtime image
COPY bin\Release\netcoreapp3.1\App
WORKDIR /App
ENTRYPOINT ["dotnet", "S3-webshop.dll"]