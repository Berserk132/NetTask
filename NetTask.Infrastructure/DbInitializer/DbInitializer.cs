using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTask.Core;
using NetTask.Core.Enums;
using NetTask.Core.UnitOfWork;

namespace NetTask.Infrastructure.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _unitOfWork = unitOfWork;
        }


        public void Initialize()
        {


            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(Roles.Role_Employee).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin)).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                }, "Admin@123").GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin2@admin.com",
                    Email = "admin2@admin.com",
                    EmailConfirmed = true,
                }, "Admin@123").GetAwaiter().GetResult();

                ApplicationUser user1 = _db.Users.FirstOrDefault(u => u.Email == "admin@admin.com");
                ApplicationUser user2 = _db.Users.FirstOrDefault(u => u.Email == "admin2@admin.com");
                _userManager.AddToRoleAsync(user1, Roles.Role_Admin).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user2, Roles.Role_Admin).GetAwaiter().GetResult();


                var departments = new List<Department>
                {
                    new Department
                    {
                        Name = "IT",
                    },
                    new Department
                    {
                        Name = "Development"
                    }
                };

                _db.Departments.AddRangeAsync(departments);
                _db.SaveChangesAsync();

                _unitOfWork.Employee.Add(new Core.Employee
                {
                    FirstName = "Admin1",
                    LastName = "Admin1",
                    Salary = 1000000,
                    ImageUrl = "",
                    DepartmentId = 1,
                    UserId = user1.Id
                });
                _unitOfWork.Employee.Add(new Core.Employee
                {
                    FirstName = "Admin2",
                    LastName = "Admin2",
                    Salary = 1000000,
                    ImageUrl = "",
                    DepartmentId = 1,
                    UserId = user2.Id
                });
                _unitOfWork.Save();

            }

            return;
        }
    }
}
