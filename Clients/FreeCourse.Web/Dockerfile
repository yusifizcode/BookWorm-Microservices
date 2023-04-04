FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Clients/FreeCourse.Web/*.csproj Clients/FreeCourse.Web/ 
RUN dotnet restore Clients/FreeCourse.Web/*.csproj
COPY . .
RUN dotnet publish Clients/FreeCourse.Web/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","FreeCourse.Web.dll"]
