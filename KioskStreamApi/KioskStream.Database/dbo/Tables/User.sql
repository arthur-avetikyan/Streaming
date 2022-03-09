CREATE TABLE [dbo].[User] (
    [Id]                 INT           NOT NULL,
    [FisrtName]          VARCHAR (256) NOT NULL,
    [LastName]           VARCHAR (256) NOT NULL,
    [Username]           VARCHAR (64)  NOT NULL,
    [Email]              VARCHAR (256) NOT NULL,
    [Password]           BINARY (64)   NOT NULL,
    [IsActive]           BIT           NOT NULL,
    [IsEmailConfirmed]   BIT           NOT NULL,
    [ConfirmationCode]   BINARY (64)   NOT NULL,
    [CreatedDatetimeUTC] DATETIME2 (7) NOT NULL,
    [UpdatedDatetimeUTC] DATETIME2 (7) NULL,
    [HashingIterationCount] INT NOT NULL DEFAULT 1000, 
    [HashingSalt] BINARY(64) NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);



