FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY Services/PhotoStock/PhotoStock.API/*.csproj Services/PhotoStock/PhotoStock.API/
RUN dotnet restore Services/PhotoStock/PhotoStock.API/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/PhotoStock.API/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","PhotoStock.API.dll"]
