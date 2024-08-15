FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BMS.API/BMS.API.csproj", "BMS.API/"]
COPY ["BMS.Application/BMS.Application.csproj", "BMS.Application/"]
COPY ["BMS.Domain/BMS.Domain.csproj", "BMS.Domain/"]
COPY ["BMS.Common/BMS.Common.csproj", "BMS.Common/"]
COPY ["BMS.Infrastructure/BMS.Infrastructure.csproj", "BMS.Infrastructure/"]
COPY ["BMS.Persistence/BMS.Persistence.csproj", "BMS.Persistence/"]
RUN dotnet restore "BMS.API/BMS.API.csproj"
COPY . .
WORKDIR "/src/BMS.API"
RUN dotnet build "BMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BMS.API.dll"]
