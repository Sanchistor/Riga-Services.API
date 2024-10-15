# Use the SDK for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy project files
COPY ["riga.services.csproj", "./"]
RUN dotnet restore "./riga.services.csproj"

# Set the PATH for global tools
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the rest of the application and build it
COPY . .
RUN dotnet build "riga.services.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "riga.services.csproj" -c Release -o /app/publish

# Use the runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "riga.services.dll"]
