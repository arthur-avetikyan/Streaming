CREATE TABLE [dbo].[Permission] (
    [Id]   INT           NOT NULL,
    [Name] VARCHAR (256) NOT NULL,
    [Code] VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id] ASC)
);



