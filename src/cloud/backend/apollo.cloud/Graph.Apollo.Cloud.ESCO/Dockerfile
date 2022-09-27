#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Graph.Apollo.Cloud.ESCO/Graph.Apollo.Cloud.ESCO.csproj", "Graph.Apollo.Cloud.ESCO/"]
RUN dotnet restore "Graph.Apollo.Cloud.ESCO/Graph.Apollo.Cloud.ESCO.csproj"
COPY . .
WORKDIR "/src/Graph.Apollo.Cloud.ESCO"
RUN dotnet build "Graph.Apollo.Cloud.ESCO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Graph.Apollo.Cloud.ESCO.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Graph.Apollo.Cloud.ESCO.dll"]