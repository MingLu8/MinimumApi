IF NOT EXISTS (
        SELECT 1
        FROM sys.tables
        WHERE name = 'Person'
            AND type = 'U'
        )
BEGIN
   CREATE TABLE [dbo].[Person]
    (
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](128) NOT NULL,
        [Age] [int] NOT NULL,
        [CreatedDateUtc] [datetime2](5) NOT NULL,
        CONSTRAINT [CRIX_Person_Id] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
    )
    ON [PRIMARY];
END


IF NOT EXISTS (
        SELECT 1
        FROM sys.tables
        WHERE name = 'TestData'
            AND type = 'U'
        )
BEGIN
   CREATE TABLE [dbo].[TestData]
    (
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](128) NOT NULL,
        [Age] [int] NOT NULL,
        [CreatedDateUtc] [datetime2](5) NOT NULL,
        CONSTRAINT [CRIX_TestData_Id] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
    )
    ON [PRIMARY];
END