using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Reservations;
using Web.Models.Shared;
using Web.Models.Users;

namespace Web.Models.Reservations
{
    public class ReservationListViewModel
    {
        public string FilterCriteria { get; set; }

        public string Filter { get; set; }

        public PagerViewModel Pager { get; set; }

        public ICollection<ReservationFlightDataViewModel> Items { get; set; }
    }
}
