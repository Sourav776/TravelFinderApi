# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY TravelFinderApi/TravelFinderApi.csproj TravelFinderApi/

RUN dotnet restore TravelFinderApi/TravelFinderApi.csproj

COPY . .

RUN dotnet publish TravelFinderApi/TravelFinderApi.csproj \
    -c Release \
    -o /app/publish


# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TravelFinderApi.dll"]