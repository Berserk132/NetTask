using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetTask.Core.Enums;
using NetTask.Core.UnitOfWork;
using NetTask.Core.ViewModels;
using NetTask.Utility.ImageUploader;
using System.Security.Claims;


namespace NetTask.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Role_Admin)]
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            EmployeeViewModel employeeVM = new()
            {
                DepartmentList = _unitOfWork.Department.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Employee = new Core.Employee()
            };
            if (id == null || id == 0)
            {
                //create
                return View(employeeVM);
            }
            else
            {
                //update
                employeeVM.Employee = _unitOfWork.Employee.Get(u => u.Id == id);
                return View(employeeVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(EmployeeViewModel employeeVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var imageUploader = new ImageUploader(_webHostEnvironment);

                // Upload new image and delete old if needed
                if (file != null)
                {
                    employeeVM.Employee.ImageUrl = imageUploader.UploadImage(file, employeeVM.Employee.ImageUrl);
                }

                if (employeeVM.Employee.Id == 0)
                {
                    // Create
                    employeeVM.Employee.ManagerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    _unitOfWork.Employee.Add(employeeVM.Employee);
                }
                else
                {
                    // Update
                    _unitOfWork.Employee.Update(employeeVM.Employee);
                }

                _unitOfWork.Save();
                TempData["success"] = "Employee created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                employeeVM.DepartmentList = _unitOfWork.Department.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(employeeVM);
            }
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Core.Employee> objEmployeeList = _unitOfWork.Employee.GetAll(filter: e => e.ManagerId == User.FindFirst(ClaimTypes.NameIdentifier).Value, includeProperties: "Department");
                return Json(new { data = objEmployeeList });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(null);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var employeeToDelete = _unitOfWork.Employee.Get(u => u.Id == id);
            var userToDelete = _unitOfWork.Users.Get(u => u.Id == employeeToDelete.UserId);

            if (employeeToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            // Use ImageUploader to handle image deletion
            var imageUploader = new ImageUploader(_webHostEnvironment);
            imageUploader.DeleteImage(employeeToDelete.ImageUrl);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userToDelete.Id)
            {
                _unitOfWork.Employee.Remove(employeeToDelete);
                _unitOfWork.Users.Remove(userToDelete);
                _unitOfWork.Save();
            }

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
