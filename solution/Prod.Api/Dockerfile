FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Prod.Api/Prod.Api.csproj", "Prod.Api/"]
RUN dotnet restore "Prod.Api/Prod.Api.csproj"

# Update database
#FROM build as migrations
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --version 8.0.2 --global dotnet-ef
RUN dotnet-ef database update --project Prod.Infrastructure/ --startup-project Prod.Api/ --context Prod.Infrastructure.Persistence.ApplicationContext

COPY . .
WORKDIR "/src/Prod.Api"
RUN dotnet build "Prod.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Prod.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prod.Api.dll"]
