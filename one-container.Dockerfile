# Build Angular
FROM docker.local:5000/node:14.2.0 as build_angular
WORKDIR /client
COPY src/client/package.json .
RUN npm install
COPY src/client/ .
ARG configuration=one-container
RUN npm run build -- --configuration=${configuration} --outputPath=./dist/out --deleteOutputPath=true --extractCss=true --aot=true --buildOptimizer=true

# Build .NET
FROM docker.local:5000/mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as build_dotnet
WORKDIR /app
COPY src/server/QNomy.csproj /app/
RUN dotnet restore
COPY src/server/ .
ARG configuration=Release
RUN dotnet publish                              \
            --configuration ${configuration}    \
            --nologo                            \
            --no-restore                        \
            --output ./dist/out                 \
            ./QNomy.csproj


# # Prepare SQL Server 2019
# FROM docker.local:5000/mcr.microsoft.com/mssql/server:2019-CU4-ubuntu-16.04

# # SQL Settins
# ENV ACCEPT_EULA=Y
# ENV SA_PASSWORD=Password1
# ENV MSSQL_SA_PASSWORD=Password1
# ENV MSSQL_PID=Express

# USER root

FROM docker.local:5000/postgress:13

# Prepare image
RUN apt-get update && apt-get install -y apt-transport-https wget nginx

# Install .NET Core package
RUN wget https://packages.microsoft.com/config/ubuntu/19.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && dpkg -i packages-microsoft-prod.deb && apt-get update

# Install .NET Core SDK + Nginx
RUN apt-get -y install dotnet-sdk-3.1 

# Configure Angular + Nginx
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build_angular /client/dist/out/ /usr/share/nginx/html
COPY src/client/ngnix-custom-2.conf /etc/nginx/conf.d/default.conf
RUN chmod a+r /etc/nginx/conf.d/default.conf

# Configure WebAPI project
WORKDIR /app
COPY --from=build_dotnet /app/dist/out/ .
# ENV ASPNETCORE_URLS http://+:5000;https://+:5001

# Port 6000 blocked by Firefox: https://developer.mozilla.org/en-US/docs/Mozilla/Mozilla_Port_Blocking
ENV ASPNETCORE_URLS http://+:6001

ENV DATABASE_SERVER localhost
ENV DATABASE_TYPE POSTGRES
ENV DB_PASSWORD Password1
ENV DB_USER user01

ENV POSTGRES_USER user01
ENV POSTGRES_PASSWORD Password1

CMD docker-entrypoint.sh -c 'max_connections=200' && postgres & /usr/sbin/service nginx start && sleep 10 && dotnet QNomy.dll

EXPOSE 8081 6001

# Run it like this:
# docker run --rm -it  -p 6001:6001/tcp -p 8081:80/tcp one-container:latest