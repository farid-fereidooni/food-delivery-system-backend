FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity.Api/Identity.Api.csproj", "Services/Identity/Identity.Api/"]
COPY ["Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Services/Identity/Identity.Infrastructure/"]
COPY ["Services/Identity/Identity.Core/Identity.Core.csproj", "Services/Identity/Identity.Core/"]
COPY ["Services/RestaurantManagement/RestaurantManagement.Api/RestaurantManagement.Api.csproj", "Services/RestaurantManagement/RestaurantManagement.Api/"]
COPY ["Services/RestaurantManagement/RestaurantManagement.Infrastructure/RestaurantManagement.Infrastructure.csproj", "Services/RestaurantManagement/RestaurantManagement.Infrastructure/"]
COPY ["Services/RestaurantManagement/RestaurantManagement.Core/RestaurantManagement.Core.csproj", "Services/RestaurantManagement/RestaurantManagement.Core/"]
COPY ["Services/RestaurantManagement/RestaurantManagement.Test/RestaurantManagement.Test.csproj", "Services/RestaurantManagement/RestaurantManagement.Test/"]
COPY ["docker-compose.dcproj", "."]
COPY ["FoodDeliverySystem.sln", "."]

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "FoodDeliverySystem.sln" --verbosity n

COPY ["Services/RestaurantManagement", "Services/RestaurantManagement"]

FROM build AS publish
WORKDIR "/src/Services/RestaurantManagement/RestaurantManagement.Api"
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish  "RestaurantManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RestaurantManagement.Api.dll"]
