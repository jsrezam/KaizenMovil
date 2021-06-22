namespace Kaizen.Models
{
    public class Customer : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string FullName 
        { 
            get 
            {
                return $"{LastName} {FirstName}";
            } 
        }
        public string IdentificationCard { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public bool State { get; set; }
    }
}
