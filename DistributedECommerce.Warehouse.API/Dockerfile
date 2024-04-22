#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DistributedECommerce.Warehouse.API/DistributedECommerce.Warehouse.API.csproj", "DistributedECommerce.Warehouse.API/"]
COPY ["DistributedECommerce.Utils/DistributedECommerce.Utils.csproj", "DistributedECommerce.Utils/"]
COPY ["DistributedECommerce.Warehouse.Application/DistributedECommerce.Warehouse.Application.csproj", "DistributedECommerce.Warehouse.Application/"]
COPY ["DistributedECommerce.Order.Common/DistributedECommerce.Orders.Common.csproj", "DistributedECommerce.Order.Common/"]
COPY ["DistributedECommerce.Warehouse.Common/DistributedECommerce.Warehouse.Common.csproj", "DistributedECommerce.Warehouse.Common/"]
COPY ["DistributedECommerce.Warehouse.Domain/DistributedECommerce.Warehouse.Domain.csproj", "DistributedECommerce.Warehouse.Domain/"]
COPY ["DistributedECommerce.Warehouse.Domain.Errors/DistributedECommerce.Warehouse.Domain.Errors.csproj", "DistributedECommerce.Warehouse.Domain.Errors/"]
COPY ["DistributedECommerce.Warehouse.Infrastructure/DistributedECommerce.Warehouse.Infrastructure.csproj", "DistributedECommerce.Warehouse.Infrastructure/"]
RUN dotnet restore "./DistributedECommerce.Warehouse.API/DistributedECommerce.Warehouse.API.csproj"
COPY . .
WORKDIR "/src/DistributedECommerce.Warehouse.API"
RUN dotnet build "./DistributedECommerce.Warehouse.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DistributedECommerce.Warehouse.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DistributedECommerce.Warehouse.API.dll"]