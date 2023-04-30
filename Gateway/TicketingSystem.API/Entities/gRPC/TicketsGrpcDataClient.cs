namespace Gateway.API.Entities.gRPC
{
    public interface ITicketsGrpcDataClient
    {
        IList<MinifiedTicketResponseDto> GetAllTickets();
        MinifiedTicketResponseDto? GetTicketById(int id);
    }

    public class TicketsGrpcDataClient : ITicketsGrpcDataClient
    {
        #region Services
        private readonly IConfiguration _config;
        private readonly GrpcChannel _channel;
        private readonly GrpcTicketsServiceClient _client;
        private readonly string _serviceUrl;
        #endregion

        #region Constructors
        public TicketsGrpcDataClient(IConfiguration config)
        {
            _config = config;
            _serviceUrl = _config["GrpcTicketsServiceUrl"];
            _channel = GrpcChannel.ForAddress(_serviceUrl);
            _client = new GrpcTicketsServiceClient(_channel);
        }
        #endregion

        #region GET
        public IList<MinifiedTicketResponseDto> GetAllTickets()
        {
            try
            {
                Log.Information($"--> Calling gRPC Service [{_serviceUrl}] to get All tickets...");
                var reply = _client.GetAllTickets(new Empty());
                Log.Information($"--> {reply.Tickets.Count} tickets has been retrieved...");
                return reply.Tickets.Adapt<IList<MinifiedTicketResponseDto>>();
            }
            catch (Exception ex)
            {
                Log.Information($"--> Could not call gRPC server {ex.Message}");
                return Enumerable.Empty<MinifiedTicketResponseDto>().ToList();
            }
        }

        public MinifiedTicketResponseDto? GetTicketById(int id)
        {
            try
            {
                Log.Information($"--> Calling gRPC Service [{_serviceUrl}] to get ticket by Id: {id}...");
                var byId = new GrpcTicketId { Id = id };
                var reply = _client.GetTicket(byId);
                Log.Information($"--> ticket with id: {id} has been retrieved...");
                return reply.Adapt<MinifiedTicketResponseDto>();
            }
            catch (Exception ex)
            {
                Log.Information($"--> Could not call gRPC server {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}
