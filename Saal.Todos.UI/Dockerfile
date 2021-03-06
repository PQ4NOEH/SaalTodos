#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG REPO=mcr.microsoft.com/dotnet/core
FROM $REPO/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM $REPO/sdk:3.1-buster AS build
ENV BuildingDocker true
WORKDIR /src
COPY ["Saal.Todos.UI/Saal.Todos.UI.csproj", "Saal.Todos.UI/"]
COPY ["Saal.Todos.Repositories/Saal.Todos.Repositories.csproj", "Saal.Todos.Repositories/"]
COPY ["Saal.Todos.Contracts/Saal.Todos.Contracts.csproj", "Saal.Todos.Contracts/"]
COPY ["Saal.Todos.Services/Saal.Todos.Services.csproj", "Saal.Todos.Services/"]
RUN dotnet restore "Saal.Todos.UI/Saal.Todos.UI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "NetCoreReactHeroku.csproj" -c Release -o /app/build

FROM node:12-alpine as build-node
WORKDIR ClientApp
COPY ClientApp/package.json .
COPY ClientApp/package-lock.json .
RUN npm install
COPY ClientApp/ .
RUN npm run-script build

FROM build AS publish
RUN dotnet publish "Saal.Todos.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build-node /ClientApp/build ./ClientApp/build
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Saal.Todos.UI.dll