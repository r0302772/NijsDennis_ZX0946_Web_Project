using Music.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class ArtistDetailsViewModel
    {
        public string Name { get; set; }
        //public string Members { get; set; }

        public List<Song> Songs { get; set; }

        public virtual ICollection<SongArtist> SongsOfArtist { get; set; }
        public virtual ICollection<SongArtist> SongArtists { get; set; }
    }
}
