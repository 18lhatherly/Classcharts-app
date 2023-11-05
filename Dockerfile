FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClasschartsApp/ClasschartsApp.csproj", "ClasschartsApp/"]
RUN dotnet restore "ClasschartsApp/ClasschartsApp.csproj"
COPY . .
WORKDIR "/src/ClasschartsApp/"
RUN dotnet build "ClasschartsApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClasschartsApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClasschartsApp.dll"]
