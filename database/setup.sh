docker exec -u root -it sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrongP@ssw0rd123 -C -i /docker-entrypoint-initdb.d/init.sql
