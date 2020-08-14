using Autofac;
using AutoMapper;
using Business.Mappers;

namespace ScanerApi.Modules
{
    public class AutoMapperModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AutoMapperModule).Assembly).As<Profile>();
            builder.Register(ctx => new MapperConfiguration(cfg => AutoMapperConfiguration.Configure(cfg)))
                   .AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                   .As<IMapper>().InstancePerLifetimeScope();
        }
    }
}