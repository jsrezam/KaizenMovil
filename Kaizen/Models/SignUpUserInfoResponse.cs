using System.Collections.Generic;

namespace Kaizen.Models
{
    public class SignUpUserInfoResponse
    {
        public IList<string> Email { get; set; }
        public IList<string> UserName { get; set; }
        public IList<string> PhoneNumber { get; set; }
        public IList<string> IdentificationCard { get; set; }        
    }
}
