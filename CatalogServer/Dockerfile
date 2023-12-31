
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

EXPOSE 5000
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CatalogServer/CatalogServer.csproj", "CatalogServer/"]
RUN dotnet restore "CatalogServer/CatalogServer.csproj"
COPY . .

WORKDIR "/src/CatalogServer"

RUN dotnet build "CatalogServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CatalogServer.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["CatalogServer/books.csv", "/app"]
COPY /MasterDb/books.csv /app/MasterDb/books.csv
RUN chown -R 1000:1000 /app
USER 1000
ENTRYPOINT ["dotnet", "CatalogServer.dll"]