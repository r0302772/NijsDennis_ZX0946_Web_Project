using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music.db.Data;
using Music.db.Models;
using Music.db.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Controllers
{
    [Authorize(Roles = "admin")]
    public class SongController : Controller
    {
        private readonly MusicDbContext _context;

        public SongController(MusicDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            SongListViewModel viewModel = new SongListViewModel();
            viewModel.Songs = await _context.Songs.Include(a => a.Album)
                                                  .Include(b => b.Remixer)
                                                  .OrderByDescending(x => x.ReleaseDate)
                                                  .ToListAsync();

            viewModel.SongArtists = await _context.SongArtists.Include(x => x.Artist).ToListAsync();

            return View(viewModel);
        }

        //public IActionResult CreateOffCanvas()
        //{
        //    return View();
        //}

        #region Create
        public async Task<IActionResult> Create()
        {
            ICollection<Album> albums = await _context.Albums.ToListAsync();
            albums.Add(new Album { AlbumTitle = "Single released." });

            ICollection<Artist> remixers = await _context.Artists.ToListAsync();
            remixers.Add(new Artist { Name = "Original version." });

            CreateSongViewModel viewModel = new CreateSongViewModel()
            {
                //SelectListItem with default null value 
                Albums = new SelectList(albums.OrderBy(x => x.AlbumTitle), "ID", "AlbumTitle", 0),
                Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "ID", "Name", 0),
                Artists = new SelectList(await _context.Artists.OrderBy(x => x.Name).ToListAsync(), "ID", "Name", 1),
                Remixers = new SelectList(remixers.OrderBy(x => x.Name), "ID", "Name", 0)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSongViewModel viewModel)
        {
            Song songindb = await _context.Songs.Where(x => x.SongTitle == viewModel.SongTitle)
                                       .Where(x => x.AlbumID == viewModel.AlbumID)
                                       .Where(x => x.BPM == viewModel.BPM)
                                       .Where(x => x.EditSongLength == viewModel.EditSongLength)
                                       .Where(x => x.ExtendedSongLength == viewModel.ExtendedSongLength)
                                       .Where(x => x.GenreID == viewModel.GenreID)
                                       .Where(x => x.Key == viewModel.Key)
                                       .Where(x => x.ReleaseDate == viewModel.ReleaseDate)
                                       .Where(x => x.RemixerID == viewModel.RemixerID)
                                       .FirstOrDefaultAsync();

            if (songindb != null)
            {
                ModelState.AddModelError("DuplicateError", "Error, you tried to make a duplicate!");
            }

            if (viewModel.EditSongLength == null && viewModel.ExtendedSongLength == null)
            {
                ModelState.AddModelError("SongLenghtError", "Error, a song has at least one length!");
            }

            if (ModelState.IsValid)
            {
                Song song = new Song()
                {
                    SongTitle = viewModel.SongTitle,
                    AlbumID = (viewModel.AlbumID == 0) ? null : viewModel.AlbumID,
                    GenreID = viewModel.GenreID,
                    RemixerID = (viewModel.RemixerID == 0) ? null : viewModel.RemixerID,
                    BPM = viewModel.BPM,
                    EditSongLength = viewModel.EditSongLength,
                    ExtendedSongLength = viewModel.ExtendedSongLength,
                    Key = viewModel.Key,
                    ReleaseDate = viewModel.ReleaseDate,
                };

                _context.Add(song);
                await _context.SaveChangesAsync();

                foreach (var artist in viewModel.SelectedArtists)
                {
                    SongArtist songArtist = new SongArtist()
                    {
                        SongID = song.ID,
                        ArtistID = artist
                    };
                    _context.Add(songArtist);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ICollection<Album> albums = await _context.Albums.ToListAsync();
            albums.Add(new Album { AlbumTitle = "Single released." });

            ICollection<Artist> remixers = await _context.Artists.ToListAsync();
            remixers.Add(new Artist { Name = "Original version." });

            viewModel.Albums = new SelectList(albums.OrderBy(x => x.AlbumTitle), "ID", "AlbumTitle", 0);
            viewModel.Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "ID", "Name", 0);
            viewModel.Artists = new SelectList(await _context.Artists.OrderBy(x => x.Name).ToListAsync(), "ID", "Name", 1);
            viewModel.Remixers = new SelectList(remixers.OrderBy(x => x.Name), "ID", "Name", 0);

            return View(viewModel);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FirstOrDefaultAsync(k => k.ID == id);
            if (song == null)
            {
                return NotFound();
            }

            DeleteSongViewModel viewModel = new DeleteSongViewModel()
            {
                SongID = song.ID,
                SongTitle = song.SongTitle,
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ICollection<SongArtist> songArtists = await _context.SongArtists.Where(x => x.SongID == id).ToListAsync();

            foreach (var songArtist in songArtists)
            {
                _context.SongArtists.Remove(songArtist);
            }
            var song = await _context.Songs.FindAsync(id);
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Song song = _context.Songs.Include(x => x.Album)
                                      .Include(x => x.Genre)
                                      .Include(x => x.Remixer)
                                      .Include(x => x.SongArtists)
                                      .Where(x => x.ID == id)
                                      .FirstOrDefault();

            ICollection<SongArtist> songArtists = _context.SongArtists.Include(x => x.Artist)
                                                                      .Where(x => x.SongID == song.ID).ToList();

            if (song != null)
            {
                SongDetailsViewModel viewModel = new SongDetailsViewModel()
                {
                    SongTitle = song.SongTitle,
                    SongArtists = songArtists,
                    Album = song.Album,
                    Genre = song.Genre,
                    Remixer = song.Remixer,
                    BPM = song.BPM,
                    EditSongLength = song.EditSongLength,
                    ExtendedSongLength = song.ExtendedSongLength,
                    Key = song.Key,
                    ReleaseDate = song.ReleaseDate,
                };
                return View(viewModel);
            }
            else
            {
                SongListViewModel viewModel = new SongListViewModel()
                {
                    Songs = _context.Songs.ToList()
                };
                return View("Index", viewModel);
            }
        }

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            ICollection<Album> albums = await _context.Albums.ToListAsync();
            albums.Add(new Album { AlbumTitle = "Single released." });

            ICollection<Artist> remixers = await _context.Artists.ToListAsync();
            remixers.Add(new Artist { Name = "Original version." });

            ICollection<SongArtist> songArtists = await _context.SongArtists.Include(x => x.Artist)
                                                          .Where(x => x.SongID == song.ID).ToListAsync();

            string artistNames = "";
            foreach (var artist in songArtists)
            {
                artistNames += $"{artist.Artist.Name}, ";
            }

            EditSongViewModel viewModel = new EditSongViewModel()
            {
                SongID = song.ID,
                SongTitle = song.SongTitle,
                ArtistNames = artistNames,
                AlbumID = song.AlbumID,
                GenreID = song.GenreID,
                RemixerID = song.RemixerID,
                BPM = song.BPM,
                EditSongLength = song.EditSongLength,
                ExtendedSongLength = song.ExtendedSongLength,
                Key = song.Key,
                ReleaseDate = song.ReleaseDate,

                Albums = new SelectList(albums.OrderBy(x => x.AlbumTitle), "ID", "AlbumTitle", (song.AlbumID == null) ? 0 : song.AlbumID),
                Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "ID", "Name", song.GenreID),
                Artists = new SelectList(await _context.Artists.OrderBy(x => x.Name).ToListAsync(), "ID", "Name"),
                Remixers = new SelectList(remixers.OrderBy(x => x.Name), "ID", "Name", (song.RemixerID == null) ? 0 : song.RemixerID),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSongViewModel viewModel)
        {
            if (id != viewModel.SongID)
            {
                return NotFound();
            }

            Song songindb = await _context.Songs.Where(x => x.SongTitle == viewModel.SongTitle)
                           .Where(x => x.AlbumID == viewModel.AlbumID)
                           .Where(x => x.BPM == viewModel.BPM)
                           .Where(x => x.EditSongLength == viewModel.EditSongLength)
                           .Where(x => x.ExtendedSongLength == viewModel.ExtendedSongLength)
                           .Where(x => x.GenreID == viewModel.GenreID)
                           .Where(x => x.Key == viewModel.Key)
                           .Where(x => x.ReleaseDate == viewModel.ReleaseDate)
                           .Where(x => x.RemixerID == viewModel.RemixerID)
                           .FirstOrDefaultAsync();

            if (songindb != null)
            {
                ModelState.AddModelError("DuplicateError", "Error, you tried to make a duplicate!");
            }

            if (viewModel.EditSongLength == null && viewModel.ExtendedSongLength == null)
            {
                ModelState.AddModelError("SongLenghtError", "Error, a song has at least one length!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Song song = new Song()
                    {
                        ID = viewModel.SongID,
                        SongTitle = viewModel.SongTitle,
                        AlbumID = (viewModel.AlbumID == 0) ? null : viewModel.AlbumID,
                        GenreID = viewModel.GenreID,
                        RemixerID = (viewModel.RemixerID == 0) ? null : viewModel.RemixerID,
                        BPM = viewModel.BPM,
                        EditSongLength = viewModel.EditSongLength,
                        ExtendedSongLength = viewModel.ExtendedSongLength,
                        Key = viewModel.Key,
                        ReleaseDate = viewModel.ReleaseDate
                    };

                    _context.Update(song);
                    await _context.SaveChangesAsync();

                    ICollection<SongArtist> songArtists = await _context.SongArtists.Include(x => x.Artist)
                                              .Where(x => x.SongID == viewModel.SongID).ToListAsync();

                    foreach (var songArtist in songArtists)
                    {
                        _context.Remove(songArtist);
                    }

                    foreach (var artist in viewModel.SelectedArtists)
                    {
                        SongArtist songArtist = new SongArtist()
                        {
                            SongID = song.ID,
                            ArtistID = artist
                        };
                        _context.Add(songArtist);
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Songs.Any(x => x.ID == viewModel.SongID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }


            ICollection<Album> albums = await _context.Albums.ToListAsync();
            albums.Add(new Album { AlbumTitle = "Single released." });

            ICollection<Artist> remixers = await _context.Artists.ToListAsync();
            remixers.Add(new Artist { Name = "Original version." });

            viewModel.Albums = new SelectList(albums.OrderBy(x => x.AlbumTitle), "ID", "AlbumTitle", (viewModel.AlbumID == 0) ? 0 : viewModel.AlbumID);
            viewModel.Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "ID", "Name", viewModel.GenreID);
            viewModel.Artists = new SelectList(await _context.Artists.OrderBy(x => x.Name).ToListAsync(), "ID", "Name");
            viewModel.Remixers = new SelectList(remixers.OrderBy(x => x.Name), "ID", "Name", (viewModel.RemixerID == 0) ? 0 : viewModel.RemixerID);

            return View(viewModel);
        }

        #endregion

        [AllowAnonymous]
        public async Task<IActionResult> Search(SongListViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.SongSearch))
            {
                viewModel.Songs = await _context.Songs.Include(a => a.Album)
                                                .Include(b => b.Remixer)
                                                .OrderByDescending(x => x.ReleaseDate)
                                                .Where(x => x.SongTitle.Contains(viewModel.SongSearch)/* ||
                                                            x.SongArtist.Contains(viewModel.SongSearch)*/)
                                                .ToListAsync();
            }
            else
            {
                viewModel.Songs = await _context.Songs.Include(a => a.Album)
                                      .Include(b => b.Remixer)
                                      .OrderByDescending(x => x.ReleaseDate)
                                      .ToListAsync();
            }
            return View("Index", viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AddToCollection(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
