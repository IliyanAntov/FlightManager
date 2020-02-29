using Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entity
{
    public class Passanger
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string UCN { get; set; }

        public string PhoneNumber { get; set; }

        public string Nationality { get; set; }

        public TicketTypeEnum TicketType { get; set; }
    }
}
