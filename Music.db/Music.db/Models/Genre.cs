using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Models
{
    public class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Song> Songs { get; set; }
    }
}
