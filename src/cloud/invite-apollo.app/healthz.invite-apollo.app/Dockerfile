#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["healthz.invite-apollo.app/healthz.invite-apollo.app.csproj", "healthz.invite-apollo.app/"]
RUN dotnet restore "healthz.invite-apollo.app/healthz.invite-apollo.app.csproj"
COPY . .
WORKDIR "/src/healthz.invite-apollo.app"
RUN dotnet build "healthz.invite-apollo.app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "healthz.invite-apollo.app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "healthz.invite-apollo.app.dll"]