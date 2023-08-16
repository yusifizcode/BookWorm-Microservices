FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Payment/Payment.API/*.csproj Services/Payment/Payment.API/
RUN dotnet restore Services/Payment/Payment.API/*.csproj
COPY . .
RUN dotnet publish Services/Payment/Payment.API/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Payment.API.dll"]
