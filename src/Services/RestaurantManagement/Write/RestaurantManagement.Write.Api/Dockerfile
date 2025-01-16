FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CrossCuttings/EventBus/EventBus.Core/EventBus.Core.csproj", "CrossCuttings/EventBus/EventBus.Core/"]
COPY ["CrossCuttings/EventBus/EventBus/EventBus.csproj", "CrossCuttings/EventBus/EventBus/"]

COPY ["Services/Identity/Identity.Api/Identity.Api.csproj", "Services/Identity/Identity.Api/"]
COPY ["Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Services/Identity/Identity.Infrastructure/"]
COPY ["Services/Identity/Identity.Core/Identity.Core.csproj", "Services/Identity/Identity.Core/"]

COPY ["Services/RestaurantManagement/Write/RestaurantManagement.Write.Api/RestaurantManagement.Write.Api.csproj", "Services/RestaurantManagement/Write/RestaurantManagement.Write.Api/"]
COPY ["Services/RestaurantManagement/Write/RestaurantManagement.Write.Application/RestaurantManagement.Write.Application.csproj", "Services/RestaurantManagement/Write/RestaurantManagement.Write.Application/"]
COPY ["Services/RestaurantManagement/Write/RestaurantManagement.Write.Domain/RestaurantManagement.Write.Domain.csproj", "Services/RestaurantManagement/Write/RestaurantManagement.Write.Domain/"]
COPY ["Services/RestaurantManagement/Write/RestaurantManagement.Write.Infrastructure/RestaurantManagement.Write.Infrastructure.csproj", "Services/RestaurantManagement/Write/RestaurantManagement.Write.Infrastructure/"]
COPY ["Services/RestaurantManagement/Write/RestaurantManagement.Write.Test/RestaurantManagement.Write.Test.csproj", "Services/RestaurantManagement/Write/RestaurantManagement.Write.Test/"]

COPY ["Services/RestaurantManagement/Read/RestaurantManagement.Read.Api/RestaurantManagement.Read.Api.csproj", "Services/RestaurantManagement/Read/RestaurantManagement.Read.Api/"]
COPY ["Services/RestaurantManagement/Read/RestaurantManagement.Read.Application/RestaurantManagement.Read.Application.csproj", "Services/RestaurantManagement/Read/RestaurantManagement.Read.Application/"]
COPY ["Services/RestaurantManagement/Read/RestaurantManagement.Read.Core/RestaurantManagement.Read.Core.csproj", "Services/RestaurantManagement/Read/RestaurantManagement.Read.Core/"]
COPY ["Services/RestaurantManagement/Read/RestaurantManagement.Read.Infrastructure/RestaurantManagement.Read.Infrastructure.csproj", "Services/RestaurantManagement/Read/RestaurantManagement.Read.Infrastructure/"]
COPY ["Services/RestaurantManagement/Read/RestaurantManagement.Read.Test/RestaurantManagement.Read.Test.csproj", "Services/RestaurantManagement/Read/RestaurantManagement.Read.Test/"]

COPY ["docker-compose.dcproj", "."]
COPY ["FoodDeliverySystem.sln", "."]

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "FoodDeliverySystem.sln" --verbosity n

COPY ["CrossCuttings", "CrossCuttings"]
COPY ["Services/RestaurantManagement", "Services/RestaurantManagement"]

FROM build AS publish
WORKDIR "/src/Services/RestaurantManagement/Write/RestaurantManagement.Write.Api"
ARG BUILD_CONFIGURATION=Release
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish  "RestaurantManagement.Write.Api.csproj" -c $BUILD_CONFIGURATION -o /app /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RestaurantManagement.Write.Api.dll"]
