using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Reservations
{
    public class ReservationDataViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int NumberOfTickets { get; set; }

    }
}
