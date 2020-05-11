# Prepare SQL Server 2019
FROM mcr.microsoft.com/mssql/server:2019-latest
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Password1
ENV MSSQL_SA_PASSWORD=Password1
ENV MSSQL_PID=Enterprise
EXPOSE 1433