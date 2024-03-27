# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy csproj files for all necessary projects
COPY Apollo.SemanticSearchWorker/Apollo.SemanticSearchWorker.csproj ./Apollo.SemanticSearchWorker/
COPY Apollo.Api/Apollo.Api.csproj ./Apollo.Api/
COPY Apollo.Service.Common/Apollo.Common.csproj ./Apollo.Service.Common/
COPY Daenet.MongoDal/Daenet.MongoDal.csproj ./Daenet.MongoDal/

# Restore dependencies for the Semantic Search Worker project
RUN dotnet restore Apollo.SemanticSearchWorker/Apollo.SemanticSearchWorker.csproj

# Copy the rest of the source code
COPY Apollo.SemanticSearchWorker/ ./Apollo.SemanticSearchWorker/
COPY Apollo.Api/ ./Apollo.Api/
COPY Apollo.Service.Common/ ./Apollo.Service.Common/
COPY Daenet.MongoDal/ ./Daenet.MongoDal/

# Publish the application
RUN dotnet publish Apollo.SemanticSearchWorker/Apollo.SemanticSearchWorker.csproj -c Release -o /app

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "Apollo.SemanticSearchWorker.dll"]
