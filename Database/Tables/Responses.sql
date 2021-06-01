CREATE TABLE [dbo].[Responses]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [CreateDateTime] DATETIME NOT NULL, 
    [EndDateTime] DATETIME NULL, 
    [json] NVARCHAR(max) NULL
)
