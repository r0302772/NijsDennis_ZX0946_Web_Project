using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class AlbumController : Controller
    {
        private readonly MusicDbContext _context;

        public AlbumController(MusicDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            AlbumListViewModel viewModel = new AlbumListViewModel();
            viewModel.Albums = await _context.Albums.OrderBy(x => x.AlbumTitle).ToListAsync();
            return View(viewModel);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAlbumViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Album()
                {
                    AlbumTitle = viewModel.AlbumTitle,
                    //AlbumArtist = viewModel.AlbumArtist,
                    AlbumCover = viewModel.AlbumCover,
                    //Songs = viewModel.Songs
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

            var album = await _context.Albums.FirstOrDefaultAsync(x => x.ID == id);
            if (album == null)
            {
                return NotFound();
            }

            DeleteAlbumViewModel viewModel = new DeleteAlbumViewModel()
            {
                AlbumID = album.ID,
                AlbumTitle = album.AlbumTitle,
                //AlbumArtist = album.AlbumArtist,
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<Song> songs = _context.Songs.Include(a => a.Genre)
                                 .Include(x => x.Remixer)
                                 .Where(x => x.AlbumID == id).ToList();

            foreach (var song in songs)
            {
                song.AlbumID = null;
            }

            var album = await _context.Albums.FindAsync(id);
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            Album album = _context.Albums.Where(x => x.ID == id).FirstOrDefault();
            List<Song> songs = _context.Songs.Include(a => a.Genre)
                                             .Include(x => x.Remixer)
                                             .Where(x => x.AlbumID == id).ToList();

            ICollection<SongArtist> songArtists = _context.SongArtists.Include(x => x.Song.Album)
                                                                      .Include(x => x.Artist)
                                                                      .Where(x => x.Song.AlbumID == id)
                                                                      .ToList();
            List<string> artists = new List<string>();
            string artistName = "";

            foreach (var artist in songArtists)
            {
                artists.Add(artist.Artist.Name);
            }

            List<string> noduplicates = artists.Distinct().ToList();

            if (noduplicates.Count() != 0)
            {
                if (noduplicates.Count() == 1)
                {
                    artistName = artists[0];
                }
                else
                {
                    artistName = "Various";
                }
            }
            else
            {
                artistName = "No artists...";
            }

            if (album != null)
            {
                AlbumDetailsViewModel viewModel = new AlbumDetailsViewModel()
                {
                    AlbumTitle = album.AlbumTitle,
                    Songs = songs,
                    SongArtists = songArtists,
                    ArtistName = artistName
                };
                return View(viewModel);
            }
            else
            {
                AlbumListViewModel viewModel = new AlbumListViewModel()
                {
                    Albums = _context.Albums.ToList()
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

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            EditAlbumViewModel viewModel = new EditAlbumViewModel()
            {
                AlbumID = album.ID,
                AlbumTitle = album.AlbumTitle,
                AlbumCover = album.AlbumCover,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAlbumViewModel viewModel)
        {
            if (id != viewModel.AlbumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Album album = new Album()
                    {
                        ID = viewModel.AlbumID,
                        AlbumTitle = viewModel.AlbumTitle,
                        AlbumCover = viewModel.AlbumCover,
                    };
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Albums.Any(x => x.ID == viewModel.AlbumID))
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
            return View(viewModel);
        }
        #endregion

        [AllowAnonymous]
        public async Task<IActionResult> Search(AlbumListViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.AlbumSearch))
            {
                viewModel.Albums = await _context.Albums.Where(x => x.AlbumTitle.Contains(viewModel.AlbumSearch)).ToListAsync();
            }
            else
            {
                viewModel.Albums = await _context.Albums.ToListAsync();
            }
            return View("Index", viewModel);
        }

    }
}
