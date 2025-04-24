FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["dbs2webapp.Api/dbs2webapp.Api.csproj", "dbs2webapp.Api/"]
COPY ["dbs2webapp.Infrastructure/Data/Seed/CustomDbObjects.sql", "./dbs2webapp.Infrastructure/Data/Seed/"]
RUN dotnet restore "dbs2webapp.Api/dbs2webapp.Api.csproj"

COPY . .
WORKDIR "/src/dbs2webapp.Api"
RUN dotnet build "dbs2webapp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dbs2webapp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ["dbs2webapp.Infrastructure/Data/Seed/CustomDbObjects.sql", "./dbs2webapp.Infrastructure/Data/Seed/"]

ENTRYPOINT ["dotnet", "dbs2webapp.Api.dll"]
