ARG DOCKER_REGISTRY_HOST

# Build .NET
FROM ${DOCKER_REGISTRY_HOST}/mcr.microsoft.com/dotnet/core/sdk:3.1.202-alpine3.11 as build_dotnet
WORKDIR /app
COPY QNomy.csproj /app/
RUN dotnet restore
COPY . .
ARG configuration=Release
RUN dotnet publish                              \
            --configuration ${configuration}    \
            --nologo                            \
            --no-restore                        \
            --output ./dist/out                 \
            ./QNomy.csproj


ARG DOCKER_REGISTRY_HOST

FROM ${DOCKER_REGISTRY_HOST}/mcr.microsoft.com/dotnet/core/aspnet:3.1.4-alpine3.11
WORKDIR /app
COPY --from=build_dotnet /app/dist/out/ .
ENV ASPNETCORE_URLS http://+:5000;https://+:5001
EXPOSE 5000 5001
ENTRYPOINT ["dotnet", "QNomy.dll"]
LABEL "qnomy"="webapi"
