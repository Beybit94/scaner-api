using Autofac;
using Autofac.Integration.WebApi;
using ScanerApi.Modules;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace ScanerApi
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterModule(new DataModule());
            builder.RegisterModule(new BussinesModule());
            builder.RegisterModule(new WebModule());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var config = GlobalConfiguration.Configuration;
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}