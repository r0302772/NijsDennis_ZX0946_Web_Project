using Microsoft.AspNetCore.Mvc.Rendering;
using Music.db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class CreateSongViewModel
    {
        [Required]
        [Display(Name = "Song Title")]
        public string SongTitle { get; set; }
        [Required]
        public int BPM { get; set; }
        [Display(Name = "Edit")]
        public string EditSongLength { get; set; }
        [Display(Name = "Extended")]
        public string ExtendedSongLength { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Remixer")]
        public int? RemixerID { get; set; }
        public SelectList Remixers { get; set; }
        [Display(Name = "Album")]
        public int? AlbumID { get; set; }
        public SelectList Albums { get; set; }
        [Display(Name = "Genre")]
        public int GenreID { get; set; }
        public SelectList Genres { get; set; }
        [Display(Name = "Artist(s)")]
        public int ArtistID { get; set; }
        public SelectList Artists { get; set; }
        public int[] SelectedArtists { get; set; }

        public string DuplicateError { get; set; }
        public string SongLenghtError { get; set; }

        public virtual ICollection<SongArtist> SongArtists { get; set; }
    }
}
