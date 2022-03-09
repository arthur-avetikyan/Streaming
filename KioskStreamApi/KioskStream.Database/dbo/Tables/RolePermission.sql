CREATE TABLE [dbo].[RolePermission] (
    [Id]           INT NOT NULL,
    [RoleId]       INT NOT NULL,
    [PermissionId] INT NOT NULL,
    CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RolePermission_Permission_1] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id]),
    CONSTRAINT [FK_RolePermission_Role_0] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);



