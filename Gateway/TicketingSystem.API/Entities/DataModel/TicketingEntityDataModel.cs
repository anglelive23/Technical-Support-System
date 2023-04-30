using Gateway.API.Entities.Dtos.Response;

namespace Gateway.API.Entities.DataModel
{
    public class TicketingEntityDataModel
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<MinifiedTicketResponseDto>("Tickets");
            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }
    }
}
