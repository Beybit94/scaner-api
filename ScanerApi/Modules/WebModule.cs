using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace ScanerApi.Modules
{
    public class WebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}