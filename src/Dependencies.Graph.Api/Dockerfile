#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80 443
VOLUME ["logs"]
ENV ASPNETCORE_URLS="http://+:80;https://+:443"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Dependencies.Graph.Api/Dependencies.Graph.Api.csproj", "Dependencies.Graph.Api/"]
COPY ["Dependencies.Graph/Dependencies.Graph.csproj", "Dependencies.Graph/"]
COPY ["Dependencies.Graph.Dtos/Dependencies.Graph.Dtos.csproj", "Dependencies.Graph.Dtos.Graph/"]

RUN dotnet restore "Dependencies.Graph.Api/Dependencies.Graph.Api.csproj"
COPY . .
WORKDIR "/src/Dependencies.Graph.Api"
RUN dotnet build "Dependencies.Graph.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dependencies.Graph.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dependencies.Graph.Api.dll"]