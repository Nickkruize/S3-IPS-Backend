FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /src
COPY ["S3-webshop/S3-webshop.csproj", "S3-webshop/"]
RUN dotnet restore "S3-webshop/S3-webshop.csproj"
COPY . .
WORKDIR "/src/S3-webshop"
RUN dotnet build "S3-webshop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "S3-webshop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "S3-webshop.dll"]
