using Music.db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class AlbumDetailsViewModel
    {
        [Display(Name = "Title")]
        public string AlbumTitle { get; set; }
        [Display(Name = "Album Cover")]
        public string AlbumCover { get; set; }

        public List<Song> Songs { get; set; }
        [Display(Name = "Artist(s)")]
        public ICollection<SongArtist> SongArtists { get; set; }

        public string ArtistName { get; set; }
    }
}
