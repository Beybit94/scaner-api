CREATE TABLE [dbo].[Inventory_Tasks]
(
	[Id] [nvarchar](50) NULL,
	[DocNum] [nvarchar](50) NULL,
	[OrderDate] [datetime] NULL,
	[LocationFrom] [nvarchar](50) NULL,
	[LocationFromId] [int] NULL,
	[LocationTo] [nvarchar](50) NULL,
	[LocationToId] [int] NULL,
	[DivisionaName] [nvarchar](50) NULL,
	[Quantity] [numeric](18, 0) NULL,
	[Volume] [numeric](18, 4) NULL,
	[OperType] [uniqueidentifier] NULL,
	[LocationTO1c] [nvarchar](50) NULL,
	[LocationTO1cc] [binary](50) NULL,
	[ROT] [nvarchar](50) NULL,
	[PlanNum] [nvarchar](50) NULL
) ON [PRIMARY]
