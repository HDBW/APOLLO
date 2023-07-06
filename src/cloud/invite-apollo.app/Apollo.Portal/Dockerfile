FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Cloud/invite-apollo.app/Apollo.Portal/Apollo.Portal.csproj", "Portal/"]
RUN dotnet restore "Cloud/invite-apollo.app/Apollo.Portal/Apollo.Portal.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet build "Apollo.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Apollo.Portal.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /app
COPY --from=publish /app/publish/wwwroot .
COPY /Client/nginx.conf /etc/nginx/nginx.conf