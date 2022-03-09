CREATE TABLE [dbo].[UserRole] (
    [Id]     INT NOT NULL,
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRole_Role_1] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [FK_UserRole_User_0] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



