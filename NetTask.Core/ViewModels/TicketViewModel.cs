using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetTask.Core.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TicketStatusList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
    }
}
