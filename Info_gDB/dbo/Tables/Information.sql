CREATE TABLE [dbo].[Information] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Text]  NVARCHAR(MAX)  NULL,
    [Image] VARBINARY(MAX) NULL,
    [Topic_Id] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),

    FOREIGN KEY ([Topic_Id]) REFERENCES [Topic]([Id])

);

