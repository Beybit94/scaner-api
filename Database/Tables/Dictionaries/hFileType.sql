﻿CREATE TABLE [dbo].[hFileType]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(255) NOT NULL, 
    [Code] NVARCHAR(250) NOT NULL UNIQUE
)

GO
