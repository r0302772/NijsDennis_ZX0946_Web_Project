using Music.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class SongListViewModel
    {
        public List<Song> Songs { get; set; }
        public string SongSearch { get; set; }

        public virtual ICollection<SongArtist> SongArtists { get; set; }
        public virtual ICollection<UserCollection> UserCollectionSongs { get; set; }
    }
}
