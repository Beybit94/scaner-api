using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010091438)]
    public class SeedData : Migration
    {
        public override void Down()
        {
            Execute.Sql(@"
delete from hFileType
delete from hhProcessType
delete from hTaskStatus");
        }

        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO hFileType (Name, Code) VALUES (N'Фото акта задачи', N'Act_Photo')
INSERT INTO hFileType (Name, Code) VALUES (N'Фото дефекта', N'Defect_Photo')

INSERT INTO hProcessType(Name, Code) VALUES (N'Старт задачи', N'Task_Start')
INSERT INTO hProcessType(Name, Code) VALUES (N'Старт коробки', N'Box_Start')
INSERT INTO hProcessType(Name, Code) VALUES (N'Окончание задачи', N'Task_End')
INSERT INTO hProcessType(Name, Code) VALUES (N'Окончание коробки', N'Box_End')

INSERT INTO hTaskStatus(Name, Code) VALUES (N'Старт', N'Start')
INSERT INTO hTaskStatus(Name, Code) VALUES (N'В процессе', N'In process')
INSERT INTO hTaskStatus(Name, Code) VALUES (N'Окончание', N'End')");
        }
    }
}
