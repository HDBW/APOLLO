#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["training.api.invite-apollo.app/training.api.invite-apollo.app.csproj", "training.api.invite-apollo.app/"]
RUN dotnet restore "training.api.invite-apollo.app/training.api.invite-apollo.app.csproj"
COPY . .
WORKDIR "/src/training.api.invite-apollo.app"
RUN dotnet build "training.api.invite-apollo.app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "training.api.invite-apollo.app.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "training.api.invite-apollo.app.dll"]
