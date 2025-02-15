using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTask.Core;
using NetTask.Core.Enums;
using NetTask.Core.UnitOfWork;
using NetTask.Core.ViewModels;
using NetTask.Infrastructure;
using SQLitePCL;

namespace NetTask.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Role_Admin)]
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public DepartmentController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Deparment created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Department departmentFromDb = _unitOfWork.Department.Get(u => u.Id == id);

            if (departmentFromDb == null)
            {
                return NotFound();
            }
            return View(departmentFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Department updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _context.Departments
                .GroupJoin(
                    _context.Employees,
                    department => department.Id,
                    employee => employee.DepartmentId,
                    (department, employees) => new DepartmentEmployeeViewModel
                    {
                        DepartmentId = department.Id,
                        DepartmentName = department.Name,
                        EmployeeCount = employees.Count(),
                        TotalSalary = employees.Sum(e => e.Salary)
                    })
                .ToListAsync();
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(null);
        }

        [HttpDelete]
        public IActionResult Delete(int? id, int count)
        {
            var departmentToDelete = _unitOfWork.Department.Get(u => u.Id == id);
            if (departmentToDelete == null || count > 0)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Department.Remove(departmentToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
