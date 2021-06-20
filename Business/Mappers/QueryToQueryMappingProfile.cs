using AutoMapper;
using Business.QueryModels.Data1c;
using Business.QueryModels.Good;
using Business.QueryModels.Logs;
using Business.QueryModels.Task;
using Business.QueryModels.Users;
using Data.Queries.Data1c;
using Data.Queries.Good;
using Data.Queries.Logs;
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
            CreateMap<TaskQueryModel, Data1cQuery>();
            CreateMap<Data1cQueryModel, Data1cQuery>();
            CreateMap<LogsListQueryModel, LogsListQuery>();
        }
        public override string ProfileName => "QueryToQueryMappingProfile";
    }
}
