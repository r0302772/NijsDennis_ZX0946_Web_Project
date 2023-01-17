using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class GrantPermissionsViewModel
    {
        public SelectList Users { get; set; }
        public SelectList Roles { get; set; }

        public string UserID { get; set; }
        public string RoleID { get; set; }
    }
}
