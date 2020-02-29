using Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Flights
{
    public class FlightsReserveViewModel
    {

        public int FlightId { get; set; }

        public int TicketNum { get; set; }

        public List<PassangerDataViewModel> Passangers { get; set; }

    }
}
