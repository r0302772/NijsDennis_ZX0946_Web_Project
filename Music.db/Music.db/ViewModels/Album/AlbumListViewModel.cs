using Music.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class AlbumListViewModel
    {
        public List<Album> Albums;
        public string AlbumSearch { get; set; }
    }
}
