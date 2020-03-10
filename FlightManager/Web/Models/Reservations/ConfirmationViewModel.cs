using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Flights;

namespace Web.Models.Reservations
{
    public class ConfirmationViewModel
    {
        public string Email { get; set; }

        public string FlightSource { get; set; }

        public string FlightDestination { get; set; }

        public DateTime DepartureTime { get; set; }

        public List<PassangerDataViewModel> Passangers { get; set; }
    }
}
