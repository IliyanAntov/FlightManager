﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace Web.Models.Flights
{
    public class UserListViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<FlightUserListViewModel> Items { get; set; }
    }
}
