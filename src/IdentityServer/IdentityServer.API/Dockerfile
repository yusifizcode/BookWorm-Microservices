FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
EXPOSE 80

COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY IdentityServer/IdentityServer.API/*.csproj IdentityServer/IdentityServer.API/ 
RUN dotnet restore IdentityServer/IdentityServer.API/*.csproj
COPY . .
RUN dotnet publish IdentityServer/IdentityServer.API/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","IdentityServer.API.dll"]
