using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Models
{
    public class Album
    {
        public int ID { get; set; }
        [Display(Name = "Album Title")]
        public string AlbumTitle { get; set; }
        [Display(Name = "Album Cover")]
        public string AlbumCover { get; set; }

        public List<Song> Songs { get; set; }
    }
}
