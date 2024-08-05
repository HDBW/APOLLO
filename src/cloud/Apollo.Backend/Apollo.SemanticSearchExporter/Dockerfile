# Use SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the main project and its dependencies
COPY src/cloud/Apollo.Backend/Apollo.SemanticSearchExporter/Apollo.SemanticSearchExporter.csproj ./Apollo.SemanticSearchExporter/
COPY src/cloud/Apollo.Backend/Apollo.Api/Apollo.Api.csproj ./Apollo.Api/
COPY src/cloud/Apollo.Backend/Apollo.Service.Common/Apollo.Common.csproj ./Apollo.Service.Common/

# Restore all projects
RUN dotnet restore Apollo.SemanticSearchExporter/Apollo.SemanticSearchExporter.csproj

# Copy all source files
COPY src/cloud/Apollo.Backend/ ./

# Set the working directory to the main project's directory
WORKDIR /app/Apollo.SemanticSearchExporter

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Use runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Apollo.SemanticSearchExporter.dll"]
