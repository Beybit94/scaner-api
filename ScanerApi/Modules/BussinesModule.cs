using Autofac;
using Business.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScanerApi.Modules
{
    public class BussinesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsersManager>().InstancePerRequest();
            builder.RegisterType<TaskManager>().InstancePerRequest();
            builder.RegisterType<GoodManager>().InstancePerRequest();
            builder.RegisterType<FileManager>().InstancePerRequest();
            builder.RegisterType<LogManager>().InstancePerRequest();
        }
    }
}