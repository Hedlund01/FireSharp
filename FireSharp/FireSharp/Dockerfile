FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FireSharp/FireSharp/FireSharp.csproj", "FireSharp/FireSharp/"]
COPY ["FireSharp/FireSharp.Client/FireSharp.Client.csproj", "FireSharp/FireSharp.Client/"]
RUN dotnet restore "FireSharp/FireSharp/FireSharp.csproj"
COPY . .
WORKDIR "/src/FireSharp/FireSharp"
RUN dotnet build "FireSharp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FireSharp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FireSharp.dll"]
