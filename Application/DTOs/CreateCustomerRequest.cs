namespace Logistica.API.Application.DTOs
{
    public class CreateCustomerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public string CustomerType { get; set; } // PERSON / BUSINESS
        public string? BusinessName { get; set; }
    }
}
