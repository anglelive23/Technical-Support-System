namespace Gateway.API.Controllers
{
    [Route("api/odata")]
    [ApiController]
    public class TicketController : AuthBaseModel
    {

        #region Services
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITicketsGrpcDataClient _grpc;
        #endregion

        #region Constructors
        public TicketController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ITicketsGrpcDataClient grpc) : base(unitOfWork)
        {
            _userManager = userManager;
            _grpc = grpc;
        }
        #endregion

        #region gRPC
        [HttpGet("tickets")]
        [ProducesResponseType(200, Type = typeof(IList<MinifiedTicketResponseDto>))]
        [OutputCache(PolicyName = "Tickets")]
        [EnableQuery]
        public async Task<IActionResult> GetAllTickets()
        {
            try
            {
                Log.Information("--> Trying to get all tickets using gRPC...");
                var tickets = _grpc.GetAllTickets();

                if (!tickets.Any())
                {
                    return NotFound("Tickets is null or empty.");
                }

                Log.Information($"--> {tickets.Count} tickets has been retrieved...");
                return await Task.FromResult(Ok(tickets));
            }
            catch (Exception ex)
            {
                Log.Information($"--> Couldn't retrieve tickets using gRPC: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tickets({key})")]
        [ProducesResponseType(200, Type = typeof(MinifiedTicketResponseDto))]
        [EnableQuery]
        public async Task<IActionResult> GetTicketById(int key)
        {
            try
            {
                Log.Information($"--> Trying to get ticket by id: {key} using gRPC...");
                var ticket = _grpc.GetTicketById(key);

                if (ticket is null)
                    return NotFound();

                Log.Information($"--> ticket with Id: {ticket.Id} has been retrieved...");
                return await Task.FromResult(Ok(ticket));
            }
            catch (Exception ex)
            {
                Log.Information($"--> Couldn't retrieve client with id: {key} using gRPC: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region REST
        //[HttpGet("tickets")]
        //[ProducesResponseType(200, Type = typeof(IList<MinifiedTicketResponseDto>))]
        //[OutputCache(PolicyName = "Tickets")]
        //[EnableQuery]
        //public async Task<IActionResult> GetAllTickets()
        //{
        //    try
        //    {
        //        var tickets = await _unitOfWork.Tickets
        //            .GetAllAsync(new[] { "Department", "Project", "Priority", "Status", "Company", "Comments", "Client", "Employee" });

        //        if (tickets == null)
        //            return NotFound();

        //        return Ok(tickets.Adapt<IList<MinifiedTicketResponseDto>>());
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}



        //[HttpGet("tickets({key})")]
        //[ProducesResponseType(200, Type = typeof(MinifiedTicketResponseDto))]
        ////[OutputCache(PolicyName = "Ticket")]
        //[EnableQuery]
        //public async Task<IActionResult> GetTicketById(int key)
        //{
        //    try
        //    {
        //        var ticket = await _unitOfWork.Tickets
        //            .FindAsync(t => t.Id == key, new[] { "Department", "Project", "Priority", "Status", "Company", "Comments", "Client", "Employee" });

        //        if (ticket == null)
        //            return NotFound();

        //        return Ok(ticket.Adapt<MinifiedTicketResponseDto>());
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
        #endregion

        #region POST - REST
        [HttpPost]
        public async Task<IActionResult> PostTicket([FromBody] TicketRequestDto ticketDto, [FromQuery] string uid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(uid);
                if (user is null)
                    return BadRequest("Wrong uid.");

                if (!await _userManager.IsInRoleAsync(user, Constants.Client))
                    return Unauthorized("Unauthorized");

                var ticket = await _unitOfWork.Tickets
                    .PostAsync(ticketDto.Adapt<Ticket>());

                // CompanyId field
                ticket.CompanyId = _unitOfWork.Projects
                    .FindAsync(p => p.Id == ticket.ProjectId).Result.CompanyId;

                _unitOfWork.Save();

                var adapt = await _unitOfWork.Tickets
                    .FindAsync(t => t.Id == ticket.Id, new[] { "Department", "Project", "Priority", "Status", "Company", "Comments" });

                return Ok(adapt.Adapt<MinifiedTicketResponseDto>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
