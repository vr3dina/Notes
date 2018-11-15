CREATE TABLE [dbo].[Users] (
    [Id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [Login]    NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

