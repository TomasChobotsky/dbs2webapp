FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["dbs2webapp.Client/dbs2webapp.Client.csproj", "./dbs2webapp.Client/"]
COPY ["dbs2webapp.Application/dbs2webapp.Application.csproj", "./dbs2webapp.Application/"]
COPY ["dbs2webapp.Infrastructure/dbs2webapp.Infrastructure.csproj", "./dbs2webapp.Infrastructure/"]
COPY ["dbs2webapp.Domain/dbs2webapp.Domain.csproj", "./dbs2webapp.Domain/"]

RUN dotnet restore "./dbs2webapp.Client/dbs2webapp.Client.csproj"

COPY . .

WORKDIR /src/dbs2webapp.Client
RUN dotnet publish dbs2webapp.Client.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
COPY --from=build /app/publish /usr/share/nginx/html
COPY ["dbs2webapp.Client/nginx.conf", "/etc/nginx/nginx.conf"]

EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
