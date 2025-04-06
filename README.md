# Temporal Tables

![Banner](./docs/banner.png)

## Pre req
```bash
dotnet tool install --global dotnet-ef
```

## Commands
```bash
$env:MigrationName  = "InitDatabaseCommit";
```

Criação do arquivo de Migration
```
dotnet ef migrations `
add $env:MigrationName `
--startup-project ./src/DEPLOY.TemporalTables.API `
--project ./src/DEPLOY.TemporalTables.API `
--context DEPLOY.TemporalTables.API.Infra.Database.Persistence.DeployDbContext `
--output-dir Migrations/EF `
--verbose
```

Aplicando o arquivo de Migration gerado
```
dotnet ef database `
update $env:MigrationName `
-s ./src/DEPLOY.TemporalTables.API `
-p ./src/DEPLOY.TemporalTables.API `
-c DEPLOY.TemporalTables.API.Infra.Database.Persistence.DeployDbContext `
-v
```


```
dotnet ef migrations script `--project ./src/DEPLOY.TemporalTables.EF`
-o ./src/DEPLOY.TemporalTables.EF/Migrations/SQL/$env:MigrationName.sql
```

## Como deletar as tabelas criadas

````sql
--Pessoas SYSTEM_VERSIONING
ALTER TABLE [dbo].[Contratos_versionados] SET ( SYSTEM_VERSIONING = OFF  )
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contratos_versionados]') AND type in (N'U'))
DROP TABLE [dbo].[Contratos_versionados]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contratos_versionadosHistory]') AND type in (N'U'))
DROP TABLE [dbo].[Contratos_versionadosHistory]
GO
--Contratos SYSTEM_VERSIONING
ALTER TABLE [dbo].[Pessoas_versionados] SET ( SYSTEM_VERSIONING = OFF  )
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pessoas_versionados]') AND type in (N'U'))
DROP TABLE [dbo].[Pessoas_versionados]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HistoricoTabelaPessoa]') AND type in (N'U'))
DROP TABLE [dbo].[HistoricoTabelaPessoa]
GO
--Contratos
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contratos_simples]') AND type in (N'U'))
DROP TABLE [dbo].[Contratos_simples]
--Pessoas
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pessoas_simples]') AND type in (N'U'))
DROP TABLE [dbo].[Pessoas_simples]
````

## Connection String
```
Server=tcp:azuresqledge.database.windows.net,1433;Initial Catalog=daploy-ef-analizer;Persist Security Info=False;User ID=felipementel;Password=Abcd1234%;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```


## Docker - SQL Edge
```
https://learn.microsoft.com/en-us/azure/azure-sql-edge/disconnected-deployment
```

```
sudo docker run \
--cap-add SYS_PTRACE \
-e 'ACCEPT_EULA=1' \
-e 'MSSQL_SA_PASSWORD=Abcd1234%' \
-e MSSQL_AGENT_ENABLED=True \
-e ClientTransportType=AMQP_TCP_Only \
-e PlanId=asde-developer-on-iot-edge \
-p 1433:1433 \
--name azuresqledge \
-d \
mcr.microsoft.com/azure-sql-edge/developer
mcr.microsoft.com/azure-sql-edge
```

```
sudo docker run \
--cap-add SYS_PTRACE \
-e 'ACCEPT_EULA=1' \
-e 'MSSQL_SA_PASSWORD=Abcd1234%' \
-p 1433:1433 \
--name azuresqledge \
-d \
mcr.microsoft.com/azure-sql-edge
```

<details>
<summary>SQL EDGE</summary>
<p>

#### Show Paths Button

```docker
sudo docker exec -it azuresqledge "bash"
```

```bash
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Abcd1234%"
```

```sql
SELECT name from sys.databases;
```

#### Show Paths View

</p>
</details>


```
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetMostPopularBlogsByName')
BEGIN
    DROP PROCEDURE dbo.GetMostPopularBlogsByName;
END
GO

CREATE PROCEDURE dbo.GetMostPopularBlogsByName
    @filterByUser NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
       [PessoaId]
      ,[Nome]
      ,[Email]
      ,[Telefone]
      ,[Documento]
      ,[Endereco]
      ,[DataNascimento]
      ,[CreatedAt]
      ,[UpdatedAt]
      ,[PeriodStart]
      ,[column-fim]
    FROM
        [dbo].[Pessoas]
    WHERE
        [Nome] LIKE @filterByUser + '%'
    ORDER BY
        [CreatedAt] DESC;
END;

-- EXECUTE dbo.GetMostPopularBlogsByName @filterByUser=CANAL

```

## Docs EF Core

```
https://learn.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables?view=sql-server-ver16
```

```
https://learn.microsoft.com/en-us/ef/core/providers/sql-server/temporal-tables
```

```
https://devblogs.microsoft.com/dotnet/prime-your-flux-capacitor-sql-server-temporal-tables-in-ef-core-6-0/
```