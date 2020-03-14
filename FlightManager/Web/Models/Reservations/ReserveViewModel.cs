using Data.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Flights;

namespace Web.Models.Reservations
{
    public class ReserveViewModel
    {

        public int FlightId { get; set; }

        public int TicketNum { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Not enough available seats")]
        public int AvailableRegularSeats { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Not enough available seats")]
        public int AvailableBusinessSeats { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter a valid Email address")]
        public string Email { get; set; }

        public List<PassangerDataViewModel> Passangers { get; set; }

    }
}
