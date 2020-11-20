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
            CreateMap<Differences, DifferencesModel>();
            CreateMap<ScanerFile,ScanerFileModel>();
            CreateMap<Scaner_1cDocData, Scaner_1cDocDataModel>();
        }

        public override string ProfileName => "EntityToModelMappingProfile";
    }
}
