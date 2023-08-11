CREATE TABLE [dbo].[Topic] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50) DEFAULT ('Unknown') NULL,
    [Information_Id] INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

