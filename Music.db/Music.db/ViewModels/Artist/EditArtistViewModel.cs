using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class EditArtistViewModel
    {
        public int ArtistID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Members { get; set; }
    }
}
