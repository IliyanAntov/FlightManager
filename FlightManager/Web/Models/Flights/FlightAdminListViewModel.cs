using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Users
{
    public class FlightAdminListViewModel
    {
        public int Id { get; set; }

        public string LocationFrom { get; set; }

        public string LocationTo { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime LandingTime { get; set; }

        public string PlaneType { get; set; }

        public long PlaneNumber { get; set; }

        public string PilotName { get; set; }

        public int RegularSeats { get; set; }

        public int BusinessSeats { get; set; }

    }
}
