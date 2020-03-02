using Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entity
{
    public class Reservation
    {
        public int Id { get; set; }

        public int FlightId { get; set; }

        public string Email { get; set; }

        public List<Passanger> Passangers { get; set; }

    }
}
