namespace NetTask.Core.Repository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        void Update(Ticket obj);
    }
}
