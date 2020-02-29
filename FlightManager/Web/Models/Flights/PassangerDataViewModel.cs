using Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Flights
{
    public class PassangerDataViewModel
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string UCN { get; set; }

        public string PhoneNumber { get; set; }

        public string Nationality { get; set; }

        public TicketTypeEnum TicketType { get; set; }
    }
}
