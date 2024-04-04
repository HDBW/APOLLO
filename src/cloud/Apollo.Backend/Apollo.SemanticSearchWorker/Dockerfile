# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["Apollo.SemanticSearchWorker/Apollo.SemanticSearchWorker.csproj", "Apollo.SemanticSearchWorker/"]
RUN dotnet restore "Apollo.SemanticSearchWorker/Apollo.SemanticSearchWorker.csproj"

# Copy the project files and build the application
COPY . .
RUN dotnet publish "Apollo.SemanticSearchWorker/Apollo.SemanticSearchWorker.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Apollo.SemanticSearchWorker.dll"]
