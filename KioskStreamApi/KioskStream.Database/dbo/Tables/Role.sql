CREATE TABLE [dbo].[Role] (
    [Id]                 INT           NOT NULL,
    [Name]               VARCHAR (256) NOT NULL,
    [CreatedByUserId]    INT           NULL,
    [CreatedDatetimeUTC] DATETIME2 (7) NOT NULL,
    [UpdatedByUserId]    INT           NULL,
    [UpdatedDatetimeUTC] DATETIME2 (7) NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Role_User_0] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Role_User_1] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[User] ([Id])
);



