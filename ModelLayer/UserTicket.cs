using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class UserTicket
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Token { get; set; }
        public DateTime createdAt { get; set; }
    }
}
