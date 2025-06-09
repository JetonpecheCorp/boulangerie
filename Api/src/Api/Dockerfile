FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# --- C'EST ICI LA MODIFICATION CRUCIALE ---
# 1. Crée le répertoire /app/Rsa
# 2. Change le propriétaire du répertoire à 'app:app' (utilisateur et groupe)
# 3. Définit les permissions à 700 : lecture/écriture/exécution pour le propriétaire 'app', rien pour les autres
RUN mkdir -p /app/Rsa && chown app:app /app/Rsa && chmod 700 /app/Rsa

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Api.csproj", "./"]
RUN dotnet restore "Api.csproj"

COPY . .
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
