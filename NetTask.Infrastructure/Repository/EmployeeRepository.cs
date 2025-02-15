using Microsoft.EntityFrameworkCore;
using NetTask.Core;
using NetTask.Core.Repository;
using NetTask.Infrastructure.Repository;

namespace NetTask.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private ApplicationDbContext _db;
        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Employee obj)
        {
            var objFromDb = _db.Employees.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.FirstName = obj.FirstName;
                objFromDb.LastName = obj.LastName;
                objFromDb.Salary = obj.Salary;
                objFromDb.ImageUrl = obj.ImageUrl;
                objFromDb.UserId = objFromDb.UserId;
                objFromDb.DepartmentId = obj.DepartmentId;
            }
        }
    }
}