using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class UserDetailsViewModel
    {
        public string UserID { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "First Name")]
        public string Firstname { get; set; }
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string RoleID { get; set; }
        public IdentityRole Role { get; set; }
    }
}
