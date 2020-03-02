using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Users
{
    public class ReservationDataViewModel
    {
        public int Id { get; set; }

        public string FlightSource { get; set; }

        public string FlightDestination { get; set; }

        public string DepartureTime { get; set; }

        public string Email { get; set; }

        public int NumberOfTickets { get; set; }

    }
}
