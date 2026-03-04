# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first for layer caching
COPY LifeManager.sln .
COPY src/LifeManager.API/LifeManager.API.csproj src/LifeManager.API/
COPY src/LifeManager.Application/LifeManager.Application.csproj src/LifeManager.Application/
COPY src/LifeManager.Domain/LifeManager.Domain.csproj src/LifeManager.Domain/
COPY src/LifeManager.Infrastructure/LifeManager.Infrastructure.csproj src/LifeManager.Infrastructure/

RUN dotnet restore src/LifeManager.API/LifeManager.API.csproj

# Copy the rest of the source
COPY src/ src/

# Publish
RUN dotnet publish src/LifeManager.API/LifeManager.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create logs directory
RUN mkdir -p /app/logs

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "LifeManager.API.dll"]
