# Prepare SQL Server 2019
FROM mcr.microsoft.com/mssql/server:2019-CU4-ubuntu-16.04
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Password1
ENV MSSQL_SA_PASSWORD=Password1
ENV MSSQL_PID=Express
EXPOSE 1433