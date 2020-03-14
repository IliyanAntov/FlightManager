using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entity
{
    public class Flight
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
