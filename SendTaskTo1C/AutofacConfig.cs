using Autofac;
using AutoMapper;
using Business.Manager;
using Business.Mappers;
using Common.Configuration;
using Data.Access;
using Data.Repositories;

namespace SendTaskTo1C
{
    public class AutofacConfig
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            builder.Register(ctx => new MapperConfiguration(cfg => AutoMapperConfiguration.Configure(cfg)))
                   .AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                   .As<IMapper>();

            builder.Register(ctx =>
            {
                var unitOfWork = new UnitOfWork(ConfigurtionOptions.MainConnectionString);
                unitOfWork.Init();
                return unitOfWork;
            }).As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<TaskRepository>();
            builder.RegisterType<Data1cRepository>();
            builder.RegisterType<TaskManager>();


            return builder.Build();
        }
    }
}
