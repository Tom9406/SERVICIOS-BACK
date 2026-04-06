namespace Logistica.API.Application.DTOs
{
    public class CustomerResponse
    {
        public int CustomerID { get; set; }
        public int CompanyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public string CustomerType { get; set; }
        public string BusinessName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
