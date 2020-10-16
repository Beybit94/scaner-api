using AutoMapper;
using Business.Models;
using Data.Model;

namespace Business.Mappers
{
    public class ModelToEntityMappingProfile : Profile
    {
        public ModelToEntityMappingProfile()
        {
            CreateMap<UsersModel, Users>();
            CreateMap<TasksModel, Tasks>();
            CreateMap<GoodsModel, Goods>();
            CreateMap<DifferencesModel, Differences>();
            CreateMap<ScanerFileModel, ScanerFile>();
        }

        public override string ProfileName => "ModelToEntityMappingProfile";
    }
}
