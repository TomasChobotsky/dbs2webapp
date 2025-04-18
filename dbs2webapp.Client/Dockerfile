# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage using nginx to serve Blazor WASM
FROM nginx:alpine AS final
COPY --from=build /app/out /usr/share/nginx/html

# Replace nginx default config
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
