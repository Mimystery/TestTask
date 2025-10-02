FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY TestTask.sln ./
COPY TestTask.API/*.csproj TestTask.API/
RUN dotnet restore

COPY . ./
RUN dotnet publish TestTask.API/TestTask.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "TestTask.API.dll"]
