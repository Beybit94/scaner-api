using AutoMapper;
using Business.Models;
using Data.Model;

namespace Business.Mappers
{
    public class EntityToModelMappingProfile : Profile
    {
        public EntityToModelMappingProfile()
        {
            CreateMap<Users, UsersModel>();
            CreateMap<Tasks, TasksModel>();
            CreateMap<Goods, GoodsModel>();
        }

        public override string ProfileName => "EntityToModelMappingProfile";
    }
}
