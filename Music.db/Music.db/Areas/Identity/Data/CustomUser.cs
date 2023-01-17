using Microsoft.AspNetCore.Identity;
using Music.db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Areas.Identity.Data
{
    public class CustomUser: IdentityUser
    {
        [PersonalData]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }
        [PersonalData]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        //under construction
        public virtual ICollection<UserCollection> UserCollectionSongs { get; set; }
    }
}
