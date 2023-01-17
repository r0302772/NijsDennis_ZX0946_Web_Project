using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.ViewModels
{
    public class EditGenreViewModel
    {
        public int GenreID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
