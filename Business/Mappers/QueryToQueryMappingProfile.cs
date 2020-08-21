using AutoMapper;
using Business.QueryModels.Good;
using Business.QueryModels.Task;
using Business.QueryModels.Users;
using Data.Queries.Good;
using Data.Queries.Task;
using Data.Queries.Users;

namespace Business.Mappers
{
    public class QueryToQueryMappingProfile:Profile
    {
        public QueryToQueryMappingProfile()
        {
            CreateMap<UsersQueryModel, UsersQuery>();
            CreateMap<TaskQueryModel, TaskQuery>();
            CreateMap<GoodQueryModel, GoodQuery>();
        }
        public override string ProfileName => "QueryToQueryMappingProfile";
    }
}
