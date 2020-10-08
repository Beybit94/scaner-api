using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Business.Manager;
using Business.Mappers;
using Data.Repositories;
using ScanerApi.Areas.Web.App_Start;
using Data.Access;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace ScanerApi.Areas.Web
{
    public class WebAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Web";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Web_default",
                "Web/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

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
            builder.RegisterType<TaskManager>().InstancePerRequest();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            WebPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}