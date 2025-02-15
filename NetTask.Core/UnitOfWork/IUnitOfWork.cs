using NetTask.Core.Repository;

namespace NetTask.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employee { get; }
        IDepartmentRepository Department { get; }
        IUserRepository Users { get; }
        ITicketRepository Ticket { get; }

        void Save();
    }
}
