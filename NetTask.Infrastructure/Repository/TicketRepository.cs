using NetTask.Core;
using NetTask.Core.Repository;
using NetTask.Infrastructure.Repository;


namespace NetTask.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly ApplicationDbContext _db;

        public TicketRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Ticket obj)
        {
            var objFromDb = _db.Tickets.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Status = obj.Status;
                objFromDb.EmployeeId = obj.EmployeeId == 0 ? objFromDb.EmployeeId : obj.EmployeeId;
            }
        }
    }
}