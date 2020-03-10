using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Flights
{
    public class FlightUserListViewModel
    {
        public int Id { get; set; }

        public string LocationFrom { get; set; }

        public string LocationTo { get; set; }

        public DateTime DepartureTime { get; set; }

        public string Duration { get; set; }

        public string PlaneType { get; set; }

        public int RegularSeats { get; set; }

        public int BusinessSeats { get; set; }
    }
}
