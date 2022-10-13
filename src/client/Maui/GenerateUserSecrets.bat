@ECHO OFF
TYPE ..\..\..\..\Apollo.json | dotnet user-secrets set -p UI\De.HDBW.Apollo.Client\De.HDBW.Apollo.Client.csproj 