using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace DatabaseMigrations.Migrations
{
    [Migration(202011301300)]
    public class GoodProcess: Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO hProcessType (Name, Code) VALUES (N'Не начато', N'NotStarted')

INSERT INTO hProcessType (Name, Code) VALUES (N'Добавление товара/короба', N'CreateGood')
INSERT INTO hProcessType (Name, Code) VALUES (N'Удаление товара/короба', N'DeleteGood')
INSERT INTO hProcessType (Name, Code) VALUES (N'Редактирование товара', N'UpdateGood')
INSERT INTO hProcessType (Name, Code) VALUES (N'Дефект товара/короба', N'Defect')
INSERT INTO hProcessType (Name, Code) VALUES (N'Отмена дефекта товара/короба', N'Undefect')");
        }
    }
}
