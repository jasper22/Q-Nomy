ARG DOCKER_REGISTRY_HOST

# Build .NET
FROM ${DOCKER_REGISTRY_HOST}/mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as build_dotnet
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

FROM ${DOCKER_REGISTRY_HOST}/mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build_dotnet /app/dist/out/ .

ENV ASPNETCORE_URLS http://+:5000
# ENV ASPNETCORE_URLS http://+:5000;https://+:5001

#ENV DATABASE_SERVER localhost
#ENV DATABASE_TYPE POSTGRES
#ENV DB_PASSWORD Password1
#ENV DB_USER user01

EXPOSE 5000 5001
CMD ["dotnet", "QNomy.dll"]
# CMD "/bin/sh"
# CMD tail -f /dev/null

LABEL "qnomy"="webapi"
