#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Products.API/Products.API.csproj", "src/Products.API/"]
COPY ["src/Products.Infrastructure/Products.Infrastructure.csproj", "src/Products.Infrastructure/"]
COPY ["src/Products.Domain/Products.Domain.csproj", "src/Products.Domain/"]
RUN dotnet restore "src/Products.API/Products.API.csproj"
COPY . .
WORKDIR "/src/src/Products.API"
RUN dotnet build "Products.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Products.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.API.dll"]
