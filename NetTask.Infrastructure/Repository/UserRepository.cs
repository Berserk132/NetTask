using Microsoft.EntityFrameworkCore;
using NetTask.Core;
using NetTask.Core.Repository;
using NetTask.Infrastructure.Repository;

namespace NetTask.Infrastructure.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser obj)
        {
            _db.Users.Update(obj);
        }
    }
}