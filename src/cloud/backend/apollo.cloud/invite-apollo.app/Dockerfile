FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY invite.apollo.app.csproj .
RUN dotnet restore invite.apollo.app.csproj
COPY . .
RUN dotnet build invite.apollo.app.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish invite.apollo.app.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf

# Set one or more individual labels
LABEL app.invite-apollo.version="0.0.1-beta"
LABEL vendor1="HDBW"
LABEL com.example.release-date="2022-11-04"
LABEL com.example.version.is-production="false"
