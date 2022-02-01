# EF Core 5.0

## `Include` Behavior

```sql
--Executed DbCommand (18ms) [Parameters=[@__name_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']

SELECT [t].[Id], [t].[Name], [t0].[Id], [t0].[EmployeeId], [t0].[End], [t0].[Start]
FROM (
    SELECT TOP(1) [e].[Id], [e].[Name]
    FROM [Employees] AS [e]
    WHERE [e].[Name] = @__name_0
    ) AS [t]
    LEFT JOIN [TimeEntries] AS [t0] ON [t].[Id] = [t0].[EmployeeId]
ORDER BY [t].[Id], [t0].[Id]
```

## Many to many

```sql
CREATE TABLE Timesheet_5_0.dbo.Employees (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Employees PRIMARY KEY (Id)
);

CREATE TABLE Timesheet_5_0.dbo.[User] (
	Id int IDENTITY(1,1) NOT NULL,
	CONSTRAINT PK_User PRIMARY KEY (Id)
);

CREATE TABLE Timesheet_5_0.dbo.CustomerUser (
	CustomerId int NOT NULL,
	UsersId int NOT NULL,
	CONSTRAINT PK_CustomerUser PRIMARY KEY (CustomerId,UsersId),
	CONSTRAINT FK_CustomerUser_Customer_CustomerId FOREIGN KEY (CustomerId) REFERENCES Timesheet_5_0.dbo.Customer(Id) ON DELETE CASCADE,
	CONSTRAINT FK_CustomerUser_User_UsersId FOREIGN KEY (UsersId) REFERENCES Timesheet_5_0.dbo.[User](Id) ON DELETE CASCADE
);
```

## Split queries

```sql
--info: Executed DbCommand (13ms) [Parameters=[@__name_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']
  SELECT [t].[Id], [t].[Name]
  FROM (
      SELECT TOP(1) [e].[Id], [e].[Name]
      FROM [Employees] AS [e]
      WHERE [e].[Name] = @__name_0
  ) AS [t]
  ORDER BY [t].[Id]

--info: Executed DbCommand (14ms) [Parameters=[@__name_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='30']
SELECT [t0].[Id], [t0].[EmployeeId], [t0].[End], [t0].[Start], [t].[Id]
FROM (
    SELECT TOP(1) [e].[Id]
    FROM [Employees] AS [e]
    WHERE [e].[Name] = @__name_0
    ) AS [t]
    INNER JOIN [TimeEntries] AS [t0] ON [t].[Id] = [t0].[EmployeeId]
ORDER BY [t].[Id]
```