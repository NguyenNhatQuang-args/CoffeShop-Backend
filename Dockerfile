FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["WebApp_Coffe/WebApp_Coffe.csproj", "WebApp_Coffe/"]
RUN dotnet restore "WebApp_Coffe/WebApp_Coffe.csproj"
COPY . .
WORKDIR "/src/WebApp_Coffe"
RUN dotnet build "WebApp_Coffe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp_Coffe.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp_Coffe.dll"]
