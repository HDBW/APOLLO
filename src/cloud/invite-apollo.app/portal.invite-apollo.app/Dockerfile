#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["portal.invite-apollo.app/portal.invite-apollo.app.csproj", "portal.invite-apollo.app/"]
RUN dotnet restore "portal.invite-apollo.app/portal.invite-apollo.app.csproj"
COPY . .
WORKDIR "/src/portal.invite-apollo.app"
RUN dotnet build "portal.invite-apollo.app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "portal.invite-apollo.app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "portal.invite-apollo.app.dll"]