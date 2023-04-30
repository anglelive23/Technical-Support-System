namespace TicketService.Entities.Repos
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepo<Ticket> Tickets { get; }
        IGenericRepo<Project> Projects { get; }
        bool Save();
    }
}
