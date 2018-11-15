CREATE PROCEDURE [dbo].[SaveNote]
	@Title nvarchar(100),
	@Published bit,
	@Text nvarchar(MAX),
	@Tags nvarchar(MAX),
	@CreationDate datetime,
	@UserId bigint,
	@BinaryFile varbinary(MAX),
	@FileType nchar(100)
AS
	INSERT INTO Note ([Title], [Published], [Text], [Tags], [CreationDate], [UserId], [BinaryFile], [FileType])
	VALUES (@Title, @Published, @Text, @Tags, @CreationDate, @UserId, @BinaryFile, @FileType)
RETURN 0