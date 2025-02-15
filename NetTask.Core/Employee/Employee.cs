using System.ComponentModel.DataAnnotations.Schema;

namespace NetTask.Core
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public ApplicationUser Manager { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}
