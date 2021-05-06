using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202105021337)]
    public class TaskClose : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO hProcessType (Name, Code) VALUES (N'Закрытие задачи', N'Task_Close')
INSERT INTO hProcessType (Name, Code) VALUES (N'Добавление товара/короба через поиск', N'CreateGood_Search')
INSERT INTO hProcessType (Name, Code) VALUES (N'Не найден', N'NotFound')
INSERT INTO hProcessType (Name, Code) VALUES (N'Расхождение', N'Diff')
INSERT INTO hProcessType (Name, Code) VALUES (N'Генерация акта', N'Task_Act')
INSERT INTO hProcessType (Name, Code) VALUES (N'Прикрепление файлов', N'File')");
        }
    }
}
