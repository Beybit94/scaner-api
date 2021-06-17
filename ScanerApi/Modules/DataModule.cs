using Autofac;
using Common.Configuration;
using Data.Access;
using Data.Repositories;
using System.Configuration;

namespace ScanerApi.Modules
{
    public class DataModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
               {
                   var con = ConfigurtionOptions.MainConnectionString;
                   var unitOfWork = new UnitOfWork(con);
                   unitOfWork.Init();
                   return unitOfWork;
               }).As<IUnitOfWork>().SingleInstance();

            builder.RegisterType<UserRepository>().InstancePerRequest();
            builder.RegisterType<TaskRepository>().InstancePerRequest();
            builder.RegisterType<GoodRepository>().InstancePerRequest();
            builder.RegisterType<Data1cRepository>().InstancePerRequest();
            builder.RegisterType<LogRepository>().InstancePerRequest();
        }
    }
}