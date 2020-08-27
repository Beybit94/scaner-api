using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ScanerApi.Areas.Web.Controllers;
using System.Reflection;

namespace ScanerApi.Modules
{
    public class WebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}