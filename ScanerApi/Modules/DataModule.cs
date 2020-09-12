using Autofac;
using Common.Configuration;
using Data.Repositories;
using ScanerApi.Data.Access;
using System.Configuration;

namespace ScanerApi.Modules
{
    public class DataModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
               {
                   var unitOfWork = new UnitOfWork(ConfigurtionOptions.MainConnectionString);
                   unitOfWork.Init();
                   return unitOfWork;
               }).As<IUnitOfWork>().SingleInstance();

            builder.RegisterType<UserRepository>().InstancePerRequest();
            builder.RegisterType<TaskRepository>().InstancePerRequest();
            builder.RegisterType<GoodRepository>().InstancePerRequest();
        }
    }
}