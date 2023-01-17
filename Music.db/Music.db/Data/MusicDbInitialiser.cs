using Music.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Data
{
    public static class MusicDbInitialiser
    {
        public static void MaakMusicDbAan(MusicDbContext context)
        {
            MusicDbContext _context = context;
            var songs = new List<Song>
            {
                new Song{SongTitle ="Casanova", EditSongLength="3:51", Key ="C# Minor", BPM = 98, ReleaseDate = new DateTime(1998, 05, 16), GenreID = 1 },
                new Song{SongTitle ="Men In Black", EditSongLength="3:51", Key ="C# Minor", BPM = 107, ReleaseDate = new DateTime(2000, 05, 16), GenreID = 1},
                new Song{SongTitle ="Freak Out", EditSongLength="3:51", Key ="B# Major", BPM = 110, ReleaseDate = new DateTime(2011, 05, 16), GenreID = 2},
                new Song{SongTitle ="Barbie Girl", EditSongLength="3:17", Key ="C Major", BPM = 108, ReleaseDate = new DateTime(1996, 05, 16), GenreID = 3},
                new Song{SongTitle ="Samba De Janeiro", EditSongLength="2:47", Key ="A# Minor", BPM = 120, ReleaseDate = new DateTime(1992, 05, 16), GenreID = 1}
            };

            var artists = new List<Artist>
            {
                new Artist{ Name = "Ultimate Kaos"},
                new Artist{ Name = "Will Smith"},
                new Artist{ Name = "2 Fabiola"},
                new Artist{ Name = "Aqua"},
                new Artist{ Name = "Bellini"},
                new Artist{ Name = "Frans Bauer"},
                new Artist{ Name = "Backstreet Boys"},
                new Artist{ Name = "Blur"},
                new Artist{ Name = "Captain Jack"},
                new Artist{ Name = "Daft Punk"},
            };

            var albums = new List<Album>
            {
                new Album{AlbumTitle="Ultimate Kaos", },
                new Album{AlbumTitle="Big Willie Style", },
                new Album{AlbumTitle="Tyfoon", },
                new Album{AlbumTitle="Aquarium", },
                new Album{AlbumTitle="Samba De Janeiro", },

            };

            var genres = new List<Genre>
            {
                new Genre{Name ="Pop"},
                new Genre{Name ="Hip Hop"},
                new Genre{Name ="Rock"},
                new Genre{Name ="R&B"},
                new Genre{Name ="Reggae"},
                new Genre{Name ="Country"},
                new Genre{Name ="Funk"},
                new Genre{Name ="Soul"},
                new Genre{Name ="Folk"},
                new Genre{Name ="Jazz"}
            };

            var songArtists = new List<SongArtist>
            {
                new SongArtist{SongID = 1, ArtistID = 1},
                new SongArtist{SongID = 2, ArtistID = 2},
                new SongArtist{SongID = 3, ArtistID = 3},
                new SongArtist{SongID = 4, ArtistID = 4},
                new SongArtist{SongID = 5, ArtistID = 5},
                new SongArtist{SongID = 5, ArtistID = 6},
            };

            if (!_context.Songs.Any()) songs.ForEach(s => _context.Songs.Add(s));
            if (!_context.Artists.Any()) artists.ForEach(a => _context.Artists.Add(a));
            if (!_context.Albums.Any()) albums.ForEach(a => _context.Albums.Add(a));
            if (!_context.Genres.Any()) genres.ForEach(g => _context.Genres.Add(g));
            if (!_context.SongArtists.Any()) songArtists.ForEach(g => _context.SongArtists.Add(g));

            _context.SaveChanges();
        }
    }
}
