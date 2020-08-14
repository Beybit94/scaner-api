using Autofac;
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
                   var unitOfWork = new UnitOfWork(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString);
                   unitOfWork.Init();
                   return unitOfWork;
               }).As<IUnitOfWork>().SingleInstance();

            builder.RegisterType<UserRepository>().InstancePerRequest();
        }
    }
}