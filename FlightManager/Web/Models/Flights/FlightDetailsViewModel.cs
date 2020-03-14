using Data.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;

namespace Web.Models.Flights
{
    public class FlightDetailsViewModel
    {
        public int FlightId { get; set; }

        [Display(Name = "Number of tickets to reserve:")]
        [TicketNumAttribute("RegularSeats", "BusinessSeats")]
        public int TicketNum { get; set; }

        [Display(Name = "Flight source:")]
        public string LocationFrom { get; set; }

        [Display(Name = "Flight destination:")]
        public string LocationTo { get; set; }

        [Display(Name = "Departure:")]
        public DateTime DepartureTime { get; set; }

        [Display(Name = "Landing:")]
        public DateTime LandingTime { get; set; }

        [Display(Name = "Plane type:")]
        public string PlaneType { get; set; }

        [Display(Name = "Plane number:")]
        public long PlaneNumber { get; set; }

        [Display(Name = "Pilot name:")]
        public string PilotName { get; set; }

        [Display(Name = "Available regular seats:")]
        public int RegularSeats { get; set; }

        [Display(Name = "Available business class seats:")]
        public int BusinessSeats { get; set; }

    }
}
