using Music.db.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Models
{
    public class UserCollection
    {
        public int ID { get; set; }
        public int SongID { get; set; }
        public string UserID { get; set; }

        public virtual Song Song { get; set; }
        public virtual CustomUser User { get; set; }
    }
}
