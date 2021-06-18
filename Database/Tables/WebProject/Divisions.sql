CREATE TABLE [dbo].[Divisions] (
    [Id]               INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]             NVARCHAR (250)   NOT NULL,
    [Guid]             UNIQUEIDENTIFIER NOT NULL,
    [ParentId]         INT              NULL,
    [IsDeleted]        BIT              CONSTRAINT [DF_Divisions_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Code]             NVARCHAR (10)    NULL,
    [GeoX]             NVARCHAR (100)   NULL,
    [GeoY]             NVARCHAR (100)   NULL,
    [IDRRef]           BINARY (16)      NULL,
    [PriceTypeIDRRef]  BINARY (16)      NULL,
    [CategoryRatingId] INT              NULL,
    CONSTRAINT [PK_Divisions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Divisions_Divisions] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Divisions] ([Id])
);