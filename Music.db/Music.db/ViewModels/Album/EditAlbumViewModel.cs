using Music.db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class EditAlbumViewModel
    {
        public int AlbumID { get; set; }
        [Required]
        [Display(Name = "Album Title")]
        public string AlbumTitle { get; set; }
        [Display(Name = "Album Cover")]
        public string AlbumCover { get; set; }

        //public ICollection<Song> Songs { get; set; }
    }
}
