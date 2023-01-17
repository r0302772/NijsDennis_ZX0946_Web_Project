using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class DeleteSongViewModel
    {
        public int SongID { get; set; }
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
    }
}
