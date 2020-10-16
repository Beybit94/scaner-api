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
                   var unitOfWork = new UnitOfWork(ConfigurtionOptions.MainConnectionString);
                   unitOfWork.Init();
                   return unitOfWork;
               }).As<IMainUnitOfWork>().SingleInstance();

            builder.Register(ctx =>
            {
                var unitOfWork = new WebProjectUnitOfWork(ConfigurtionOptions.WebProjectConnectionString);
                unitOfWork.Init();
                return unitOfWork;
            }).As<IWebProjectUnitOfWork>().SingleInstance();

            builder.RegisterType<UserRepository>().InstancePerRequest();
            builder.RegisterType<TaskRepository>().InstancePerRequest();
            builder.RegisterType<GoodRepository>().InstancePerRequest();
        }
    }
}