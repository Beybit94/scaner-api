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
            CreateMap<ScanerFileModel, ScanerFile>();
            CreateMap<Scaner_1cDocDataModel, Scaner_1cDocData>();
            CreateMap<LogsModel, Logs>();
        }

        public override string ProfileName => "ModelToEntityMappingProfile";
    }
}
