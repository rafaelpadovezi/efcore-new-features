# EF Core 3.1

## `Include` Behavior

```sql
-- info: Microsoft.EntityFrameworkCore.Database.Command[20101]
--      Executed DbCommand (14ms) [Parameters=[], CommandType='Text', CommandTimeout='30']

SELECT [t].[Id], [t].[Name], [t0].[Id], [t0].[EmployeeId], [t0].[End], [t0].[Start]
      FROM (
          SELECT TOP(1) [e].[Id], [e].[Name]
          FROM [Employees] AS [e]
          WHERE [e].[Name] = N'Rafael'
      ) AS [t]
      LEFT JOIN [TimeEntries] AS [t0] ON [t].[Id] = [t0].[EmployeeId]
      ORDER BY [t].[Id], [t0].[Id]
```

## Restricted Client Evaluation

```sql
SELECT [e].[Name]
      FROM [Employees] AS [e]
```