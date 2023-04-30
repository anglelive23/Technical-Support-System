using System.Reflection;

namespace TicketService.Entities.Config
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<Ticket, GrpcTicketModel>
                .NewConfig()
                .Map(dest => dest.DateCreated, src => Timestamp.FromDateTime(DateTime.SpecifyKind((src.DateCreated), DateTimeKind.Utc)))
                .Map(dest => dest.LastSeen, src => Timestamp.FromDateTime(DateTime.SpecifyKind((src.LastSeen ?? DateTime.UtcNow), DateTimeKind.Utc)))
                .Map(dest => dest.EstimatedTime, src => Timestamp.FromDateTime(DateTime.SpecifyKind((src.EstimatedTime ?? DateTime.UtcNow), DateTimeKind.Utc)));


            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
