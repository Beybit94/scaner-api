CREATE TABLE [dbo].[Logs]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
    [TaskId] INT NOT NULL, 
    [ProcessTypeId] INT NOT NULL,
    [Created] DATETIME NOT NULL DEFAULT getdate(), 
    [Message] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Logs_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks]([Id]), 
    CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId] FOREIGN KEY ([ProcessTypeId]) REFERENCES [hProcessType]([Id]) 
)
