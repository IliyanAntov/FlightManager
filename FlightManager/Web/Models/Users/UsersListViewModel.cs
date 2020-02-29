using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Shared;

namespace Web.Models.Users
{
    public class UsersListViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<UsersViewModel> Items { get; set; }
    }
}
