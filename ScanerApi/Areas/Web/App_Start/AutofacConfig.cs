using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using Business.Manager;
using Business.Mappers;
using Data.Access;
using Data.Repositories;
using ScanerApi.Modules;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace ScanerApi.Areas.Web
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.Register(ctx => new MapperConfiguration(cfg => AutoMapperConfiguration.Configure(cfg)))
                   .AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                   .As<IMapper>().InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                var unitOfWork = new UnitOfWork(ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString);
                unitOfWork.Init();
                return unitOfWork;
            }).As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<TaskRepository>().InstancePerRequest();
            builder.RegisterType<Data1cRepository>().InstancePerRequest();
            builder.RegisterType<GoodRepository>().InstancePerRequest();
            builder.RegisterType<LogRepository>().InstancePerRequest();

            builder.RegisterType<TaskManager>().InstancePerRequest();
            builder.RegisterType<GoodManager>().InstancePerRequest();
            builder.RegisterType<LogManager>().InstancePerRequest();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}