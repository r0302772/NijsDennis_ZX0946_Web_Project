using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Models
{
    public class Song
    {
        public int ID { get; set; }
        [Display(Name = "Title")]
        public string SongTitle { get; set; }
        [Display(Name = "Song Cover")]
        public string SongCover { get; set; }
        [Display(Name = "Edit")]
        public string EditSongLength { get; set; }
        [Display(Name = "Extended")]
        public string ExtendedSongLength { get; set; }
        public string Key { get; set; }
        public int BPM { get; set; }
        [Display(Name = "Release")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        //public Decimal? Price { get; set; }

        [Display(Name = "Album")]
        public int? AlbumID { get; set; }
        public Album Album { get; set; }
        [Display(Name = "Remixer")]
        public int? RemixerID { get; set; }
        public Artist Remixer { get; set; }
        [Display(Name = "Genre")]
        public int GenreID { get; set; }
        public Genre Genre { get; set; }

        [Display(Name = "Artist")]
        public virtual ICollection<SongArtist> SongArtists { get; set; }
        public virtual ICollection<UserCollection> UserCollectionSongs { get; set; }
    }
}
