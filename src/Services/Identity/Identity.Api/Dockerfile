FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity.Api/Identity.Api.csproj", "Services/Identity/Identity.Api/"]
COPY ["Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Services/Identity/Identity.Infrastructure/"]
COPY ["Services/Identity/Identity.Core/Identity.Core.csproj", "Services/Identity/Identity.Core/"]
COPY ["docker-compose.dcproj", "."]
COPY ["FoodDeliverySystem.sln", "."]

RUN dotnet restore "FoodDeliverySystem.sln" --verbosity n
COPY ["Services/Identity", "Services/Identity"]

FROM build AS publish
WORKDIR "/src/Services/Identity/Identity.Api"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish  "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
