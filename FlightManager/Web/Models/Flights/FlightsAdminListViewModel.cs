using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;
using Web.Models.Users;

namespace Web.Models.Flights
{
    public class FlightsAdminListViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<FlightsAdminViewModel> Items { get; set; }
    }
}
