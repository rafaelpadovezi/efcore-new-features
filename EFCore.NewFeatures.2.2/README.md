# EF Core 2.2

## `.Include` behavior

```sql
--Microsoft.EntityFrameworkCore.Database.Command:Information: Executed DbCommand (8ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT TOP(1) [x].[Id], [x].[Name]
FROM [Employees] AS [x]
WHERE [x].[Name] = N'Rafael'
ORDER BY [x].[Id]


--Microsoft.EntityFrameworkCore.Database.Command:Information: Executed DbCommand (12ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT [x.Entries].[Id], [x.Entries].[EmployeeId], [x.Entries].[End], [x.Entries].[Start]
FROM [TimeEntries] AS [x.Entries]
INNER JOIN (
    SELECT TOP(1) [x0].[Id]
    FROM [Employees] AS [x0]
    WHERE [x0].[Name] = N'Rafael'
    ORDER BY [x0].[Id]
) AS [t] ON [x.Entries].[EmployeeId] = [t].[Id]
ORDER BY [t].[Id]
```

## ClientEvalution

```sql
SELECT [x].[Id], [x].[Name]
FROM [Employees] AS [x]
```