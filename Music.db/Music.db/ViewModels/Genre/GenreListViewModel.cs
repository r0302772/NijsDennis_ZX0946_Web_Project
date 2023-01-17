using Music.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class GenreListViewModel
    {
        public List<Genre> Genres { get; set; }
        public string GenreSearch { get; set; }
    }
}
