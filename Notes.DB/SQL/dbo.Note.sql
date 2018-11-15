CREATE TABLE [dbo].[Note] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (100)  NOT NULL,
    [Published]    BIT             NOT NULL,
    [Text]         NVARCHAR (MAX)  NOT NULL,
    [Tags]         NVARCHAR (MAX)  NULL,
    [CreationDate] DATETIME		   NOT NULL,
    [UserId]       BIGINT          NOT NULL,
    [BinaryFile]   VARBINARY (MAX) NULL,
    [FileType]     NCHAR (100)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Note_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

