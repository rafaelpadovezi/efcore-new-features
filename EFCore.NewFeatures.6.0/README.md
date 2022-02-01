## Temporal Tables Support

```sql
CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
    [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[EmployeesHistory]))
```


```sql
SELECT [e].[Id], [e].[Name], [e].[PeriodEnd], [e].[PeriodStart]
      FROM [Employees] FOR SYSTEM_TIME ALL AS [e]
      WHERE [e].[Id] = 1
      ORDER BY [e].[PeriodEnd]
```