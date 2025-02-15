using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetTask.Core;
using NetTask.Core.Constants;
using NetTask.Core.Enums;
using NetTask.Core.UnitOfWork;
using NetTask.Core.ViewModels;
using System.Security.Claims;

namespace NetTask.Web.Areas.Home.Controllers
{
    [Area("Home")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            TicketViewModel ticketViewModel = new()
            {
                TicketStatusList = TicketStatus.Ticket_Status.Select(status => new SelectListItem
                {
                    Text = status,
                    Value = status
                }),
                EmployeeList = _unitOfWork.Employee
                    .GetAll(filter: e => e.ManagerId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                    .Select(employee => new SelectListItem
                {
                    Text = employee.FullName,
                    Value = employee.Id.ToString()
                    }),
                Ticket = new Ticket()
            };
            if (id == null || id == 0)
            {
                //create
                return View(ticketViewModel);
            }
            else
            {
                //update
                ticketViewModel.Ticket = _unitOfWork.Ticket.Get(u => u.Id == id);
                return View(ticketViewModel);
            }

        }

        [HttpPost]
        public IActionResult Upsert(TicketViewModel ticketViewModel)
        {
            if (ModelState.IsValid)
            {
                if (ticketViewModel.Ticket.Id == 0)
                {
                    //create
                    _unitOfWork.Ticket.Add(ticketViewModel.Ticket);
                }
                else
                {
                    //update
                    _unitOfWork.Ticket.Update(ticketViewModel.Ticket);
                }
                _unitOfWork.Save();
                TempData["success"] = "Ticket created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ticketViewModel.TicketStatusList = TicketStatus.Ticket_Status.Select(status => new SelectListItem
                {
                    Text = status,
                    Value = status
                });
                return View(ticketViewModel);
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var currentUserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = _unitOfWork
                    .Ticket
                    .GetAll(
                        filter: t => t.Employee.ManagerId == currentUserId || t.Employee.UserId == currentUserId, 
                        includeProperties: "Employee");
                return Json(new
                {
                    data = result.Select(t => new
                    {
                        t.Id,
                        t.Title,
                        t.EmployeeId,
                        t.Status,
                        IsAdmin = User.IsInRole(Roles.Role_Admin)
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ticketToDelete = _unitOfWork.Ticket.Get(u => u.Id == id);
            if (ticketToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Ticket.Remove(ticketToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
