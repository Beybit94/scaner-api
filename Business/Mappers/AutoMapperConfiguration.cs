using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Mappers
{
    public class AutoMapperConfiguration
    {
        public static Action<IMapperConfigurationExpression> Configure = config =>
        {
            config.AllowNullCollections = true;
            config.AllowNullDestinationValues = true;
            config.AddProfile<EntityToModelMappingProfile>();
            config.AddProfile<ModelToEntityMappingProfile>();
            config.AddProfile<QueryToQueryMappingProfile>();
        };
    }
}
