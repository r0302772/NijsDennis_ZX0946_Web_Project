using Music.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class ArtistListViewModel
    {
        public List<Artist> Artists;
        public string ArtistSearch { get; set; }
    }
}