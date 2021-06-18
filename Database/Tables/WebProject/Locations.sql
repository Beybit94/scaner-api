CREATE TABLE [dbo].[Locations] (
    [LocationId]     INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LocationCode]   NVARCHAR (10)    NULL,
    [LocationName]   NVARCHAR (250)   NULL,
    [LocationPrefix] NVARCHAR (4)     NULL,
    [LocationGuid]   UNIQUEIDENTIFIER NULL,
    [DivisionId]     INT              NULL,
    [IDRRef]         BINARY (16)      NULL,
    [IsDeleted]      INT              NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([LocationId] ASC)
);
