namespace NetTask.Core
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
