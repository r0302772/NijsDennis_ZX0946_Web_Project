using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class DeleteGenreViewModel
    {
        public int GenreID { get; set; }
        public string Name { get; set; }

        public string Error { get; set; }
    }
}
