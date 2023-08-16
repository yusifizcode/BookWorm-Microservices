FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY Shared/SharedLibrary/*.csproj Shared/SharedLibrary/
COPY Clients/Client.MVC/*.csproj Clients/Client.MVC/ 
RUN dotnet restore Clients/Client.MVC/*.csproj
COPY . .
RUN dotnet publish Clients/Client.MVC/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Client.MVC.dll"]
