using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202105011153)]
    public class LogEndTime : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
ALTER TABLE [dbo].[Logs]
    ADD [Ended]    DATETIME NULL,
        [ParentId] INT      NULL;


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Процент повреждения', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Defects', @level2type = N'COLUMN', @level2name = N'Damage';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Описание', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Defects', @level2type = N'COLUMN', @level2name = N'Description';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Серийный номер', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Defects', @level2type = N'COLUMN', @level2name = N'SerialNumber';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Дефект', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'DefectId';
");
        }
    }
}
