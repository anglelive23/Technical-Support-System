using System;
using System.Reflection;
using Gateway.API.Entities.Dtos.Response;
using TicketService;

namespace Gateway.API.Entities.Config
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<GrpcTicketModel, MinifiedTicketResponseDto>
                .NewConfig()
                .Map(dest => dest.DateCreated, src => src.DateCreated.ToDateTime())
                .Map(dest => dest.LastSeen, src => src.LastSeen == null ? DateTime.Now : src.LastSeen.ToDateTime())
                .Map(dest => dest.EstimatedTime, src => src.EstimatedTime == null ? DateTime.Now : src.EstimatedTime.ToDateTime());


            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
