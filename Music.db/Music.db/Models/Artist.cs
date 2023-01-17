using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Models
{
    public class Artist
    {
        public int ID { get; set; }
        [Display(Name = "Artist")]
        public string Name { get; set; }
        //public string Members { get; set; }

        public virtual ICollection<SongArtist> SongArtists { get; set; }
    }
}
