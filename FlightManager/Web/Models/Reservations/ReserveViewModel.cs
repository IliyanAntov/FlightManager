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

        public int AvailableRegularSeats { get; set; }

        public int AvailableBusinessSeats { get; set; }

        public string Email { get; set; }

        public List<PassangerDataViewModel> Passangers { get; set; }

    }
}
