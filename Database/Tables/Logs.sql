CREATE TABLE [dbo].[Logs]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
    [TaskId] INT NULL, 
    [ProcessTypeId] INT NOT NULL,
    [Created] DATETIME NOT NULL DEFAULT getdate(), 
    [Response] NVARCHAR(MAX) NULL, 
    [Request] NVARCHAR(MAX) NULL, 
    [Ended] DATETIME NULL, 
    [ParentId] INT NULL, 
    [GoodId] INT NULL, 
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Logs_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks]([Id]), 
    CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId] FOREIGN KEY ([ProcessTypeId]) REFERENCES [hProcessType]([Id]), 
    CONSTRAINT [FK_Logs_Scaner_Goods_GoodId] FOREIGN KEY ([GoodId]) REFERENCES [Scaner_Goods]([Id]) 
)
