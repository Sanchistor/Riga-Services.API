# Use the official Microsoft ASP.NET Core runtime image for the base image
# This image includes the runtime only, not the ASP.NET Core SDK
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

# Use the official Microsoft ASP.NET Core SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file and restore as distinct layers
COPY ["riga.services.csproj", "./"]
RUN dotnet restore "./riga.services.csproj"

# Copy everything else and build the API
COPY . .
WORKDIR "/src"
RUN dotnet build "riga.services.csproj" -c Release -o /app/build

# Publish the API
FROM build AS publish
RUN dotnet publish "riga.services.csproj" -c Release -o /app/publish

# Final stage: use the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "riga.services.dll"]
