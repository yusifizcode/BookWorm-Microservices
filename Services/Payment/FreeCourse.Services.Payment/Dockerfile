FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Payment/FreeCourse.Services.Payment/*.csproj Services/Payment/FreeCourse.Services.Payment/
RUN dotnet restore Services/Payment/FreeCourse.Services.Payment/*.csproj
COPY . .
RUN dotnet publish Services/Payment/FreeCourse.Services.Payment/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","FreeCourse.Services.Payment.dll"]
