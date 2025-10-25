# --- BUILD STAGE ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy các file .csproj trước để tối ưu cache
COPY ["HeartSpace.Api/HeartSpace.Api.csproj", "HeartSpace.Api/"]
COPY ["HeartSpace.Application/HeartSpace.Application.csproj", "HeartSpace.Application/"]
COPY ["HeartSpace.Domain/HeartSpace.Domain.csproj", "HeartSpace.Domain/"]
COPY ["HeartSpace.Infrastructure/HeartSpace.Infrastructure.csproj", "HeartSpace.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "HeartSpace.Api/HeartSpace.Api.csproj"

# Copy toàn bộ source code
COPY . .

# Build ứng dụng
WORKDIR "/src/HeartSpace.Api"
RUN dotnet build "HeartSpace.Api.csproj" -c Release -o /app/build

# --- PUBLISH STAGE ---
FROM build AS publish
RUN dotnet publish "HeartSpace.Api.csproj" -c Release -o /app/publish

# --- RUNTIME STAGE ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose cả HTTP & HTTPS
EXPOSE 8080
EXPOSE 443

ENTRYPOINT ["dotnet", "HeartSpace.Api.dll"]
