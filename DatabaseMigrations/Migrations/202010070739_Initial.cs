using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010070739)]
    public class Initial : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
CREATE TABLE [dbo].[hFileType] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    [Code] NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[hTaskStatus] (
    [Id]   INT            NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    [Code] NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[Scaner_1cDocData] (
    [Id]                 INT              NOT NULL,
    [PlanNum]            NVARCHAR (50)    NULL,
    [DateDoc]            DATETIME         NULL,
    [GUID_PlanWMSNumber] UNIQUEIDENTIFIER NULL,
    [NumberDoc]          NVARCHAR (50)    NULL,
    [TypeDoc]            NVARCHAR (50)    NULL,
    [Article]            NVARCHAR (50)    NULL,
    [Barcode]            NVARCHAR (50)    NULL,
    [Quantity]           INT              NULL,
    [UserId]             INT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[Scaner_File] (
    [Id]     INT            NOT NULL,
    [TaskId] INT            NOT NULL,
    [BoxId]  INT            NULL,
    [Path]   NVARCHAR (500) NOT NULL,
    [TypeId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[Scaner_Goods] (
    [Id]          INT            NOT NULL,
    [TaskId]      INT            NOT NULL,
    [GoodId]      INT            NULL,
    [GoodArticle] NVARCHAR (50)  NULL,
    [GoodName]    NVARCHAR (500) NULL,
    [CountQty]    INT            NULL,
    [BarCode]     NVARCHAR (50)  NULL,
    [BoxId]       INT            NULL,
    [IsDefect]    BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[Tasks] (
    [Id]             INT            NOT NULL,
    [StatusId]       INT            NOT NULL,
    [UserId]         INT            NOT NULL,
    [DivisionId]     INT            NOT NULL,
    [CreateDateTime] DATETIME       NOT NULL,
    [EndDateTime]    DATETIME       NULL,
    [PlanNum]        NVARCHAR (150) NOT NULL,
    [BarCode]        NVARCHAR (50)  NULL,
    [ParentId]       INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
ALTER TABLE [dbo].[Scaner_File] WITH NOCHECK
    ADD CONSTRAINT [FK_Scaner_File_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);

GO
ALTER TABLE [dbo].[Scaner_File] WITH NOCHECK
    ADD CONSTRAINT [FK_Scaner_File_hFileType_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[hFileType] ([Id]);

GO
ALTER TABLE [dbo].[Scaner_Goods] WITH NOCHECK
    ADD CONSTRAINT [FK_Scaner_Goods_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);

GO
ALTER TABLE [dbo].[Tasks] WITH NOCHECK
    ADD CONSTRAINT [FK_Tasks_hTaskStatus_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[hTaskStatus] ([Id]);

GO
ALTER TABLE [dbo].[Scaner_File] WITH CHECK CHECK CONSTRAINT [FK_Scaner_File_Tasks_TaskId];
ALTER TABLE [dbo].[Scaner_File] WITH CHECK CHECK CONSTRAINT [FK_Scaner_File_hFileType_TypeId];
ALTER TABLE [dbo].[Scaner_Goods] WITH CHECK CHECK CONSTRAINT [FK_Scaner_Goods_Tasks_TaskId];
ALTER TABLE [dbo].[Tasks] WITH CHECK CHECK CONSTRAINT [FK_Tasks_hTaskStatus_StatusId];");
        }
    }
}
