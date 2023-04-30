namespace TicketService.Services
{
    public class TicketsService : GrpcTicketsServiceBase
    {
        #region Included List
        private readonly string[] includedList = new string[] { "Client", "Employee", "Priority", "Department", "Project", "Status", "Company" };
        #endregion

        #region Services
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public TicketsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET
        public override async Task<GrpcTicketsRespone> GetAllTickets(Empty request, ServerCallContext context)
        {
            var tickets = await _unitOfWork.Tickets
                .GetAllAsync(new[] { "Client", "Employee", "Priority", "Department", "Project", "Status", "Company" });
            var response = new GrpcTicketsRespone();
            response.Tickets.AddRange(tickets.Adapt<IList<GrpcTicketModel>>());
            return response;
        }

        public override async Task<GrpcTicketModel> GetTicket(GrpcTicketId request, ServerCallContext context)
        {
            var ticket = await _unitOfWork.Tickets
                .FindAsync(t => t.Id == request.Id, includedList);
            return ticket.Adapt<GrpcTicketModel>();
        }
        #endregion
    }
}
