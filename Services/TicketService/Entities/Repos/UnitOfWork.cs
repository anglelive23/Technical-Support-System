

namespace TicketService.Entities.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Services
        private readonly TicketContext _context;
        #endregion

        #region Repos
        public IGenericRepo<Ticket> Tickets { get; private set; }
        public IGenericRepo<Project> Projects { get; private set; }
        #endregion

        public UnitOfWork(TicketContext context)
        {
            _context = context;
            Tickets = new GenericRepo<Ticket>(_context);
            Projects = new GenericRepo<Project>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
