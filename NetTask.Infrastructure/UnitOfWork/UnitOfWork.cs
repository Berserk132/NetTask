using NetTask.Core.Repository;
using NetTask.Core.UnitOfWork;
using NetTask.Infrastructure.Repositories;

namespace NetTask.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork

    {
        private ApplicationDbContext _db;
        public IEmployeeRepository Employee { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public ITicketRepository Ticket { get; private set; }
        public IUserRepository Users { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Employee = new EmployeeRepository(_db);
            Department = new DepartmentRepository(_db);
            Users = new UserRepository(_db);
            Ticket = new TicketRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
