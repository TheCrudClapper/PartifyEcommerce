# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Partify.UI/Partify.UI.csproj", "src/Partify.UI/"]
COPY ["src/Partify.Core/Partify.Core.csproj", "src/Partify.Core/"]
COPY ["src/Partify.Infrastructure/Partify.Infrastructure.csproj", "src/Partify.Infrastructure/"]
RUN dotnet restore "./src/Partify.UI/Partify.UI.csproj"
COPY . .
WORKDIR "/src/src/Partify.UI"
RUN dotnet build "./Partify.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Partify.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV DB_HOST=localhost
ENV DB_PORT=5432
ENV DB_USER=postgres
ENV DB_NAME=PartfyEcommerceDb 
ENV DB_PASSWORD=admin
ENTRYPOINT ["dotnet", "Partify.UI.dll"]