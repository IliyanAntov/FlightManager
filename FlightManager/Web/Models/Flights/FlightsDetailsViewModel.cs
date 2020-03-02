using Data.Enumeration;
using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;

namespace Web.Models.Flights
{
    public class FlightsDetailsViewModel
    {
        public int FlightId { get; set; }

        [TicketNumAttribute("RegularSeats", "BusinessSeats")]
        public int TicketNum { get; set; }

        public string LocationFrom { get; set; }

        public string LocationTo { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime LandingTime { get; set; }

        public string PlaneType { get; set; }

        public int PlaneNumber { get; set; }

        public string PilotName { get; set; }

        public int RegularSeats { get; set; }

        public int BusinessSeats { get; set; }

    }
}
