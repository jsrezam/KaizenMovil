using System;
using System.Collections.Generic;
using System.Text;

namespace Kaizen.Models
{
    public class UserCredentials
    {
        public string UserName { get; set; }        
        public string Email { get; set; }        
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string IdentificationCard { get; set; }
        public string PhoneNumber { get; set; }
    }
}
