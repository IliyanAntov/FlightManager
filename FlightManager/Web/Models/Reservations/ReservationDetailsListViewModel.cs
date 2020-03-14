using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Flights;
using Web.Models.Shared;

namespace Web.Models.Reservations
{
    public class ReservationDetailsListViewModel
    {
        public PagerViewModel Pager { get; set; }

        public long PlaneNum { get; set; }

        public ICollection<PassangerDataViewModel> Items { get; set; }
    }
}
