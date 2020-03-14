using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Attributes;

namespace Web.Models.Flights
{
    public class FlightEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Flight source")]
        public string LocationFrom { get; set; }

        [Required]
        [Display(Name = "Flight destination")]
        public string LocationTo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Departure")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [LandingTimeAttribute("DepartureTime")]
        [Display(Name = "Landing")]
        public DateTime LandingTime { get; set; }

        [Required]
        [Display(Name = "Plane type")]
        public string PlaneType { get; set; }

        [Required]
        [Display(Name = "Plane number")]
        public long PlaneNumber { get; set; }

        [Required]
        [Display(Name = "Pilot name")]
        public string PilotName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = ("Please enter a valid seat number"))]
        [Display(Name = "Total regular seats")]
        public int RegularSeats { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = ("Please enter a valid seat number"))]
        [Display(Name = "Total business class seats")]
        public int BusinessSeats { get; set; }
    }
}
