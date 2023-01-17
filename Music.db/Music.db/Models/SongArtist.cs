using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Models
{
    public class SongArtist
    {
        public int ID { get; set; }
        public int SongID { get; set; }
        public int ArtistID { get; set; }

        public virtual Song Song { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
