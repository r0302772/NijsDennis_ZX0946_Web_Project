using Microsoft.AspNetCore.Identity;
using Music.db.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class UserListViewModel
    {
        public List<CustomUser> Users { get; set; }
        //public List<IdentityRole> Roles { get; set; }
        public string UserSearch { get; set; }
    }
}
