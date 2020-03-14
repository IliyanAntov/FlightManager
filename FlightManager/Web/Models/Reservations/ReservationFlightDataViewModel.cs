using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Users;

namespace Web.Models.Reservations
{
    public class ReservationFlightDataViewModel
    {
        public int Id { get; set; }

        public long PlaneNum { get; set; }

        public string FlightSource { get; set; }

        public string FlightDestination { get; set; }

        public DateTime DepartureTime { get; set; }

        public List<ReservationDataViewModel> Reservations { get; set; }
    }
}
