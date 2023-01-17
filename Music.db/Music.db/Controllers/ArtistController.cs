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
    public class ArtistController : Controller
    {
        private readonly MusicDbContext _context;

        public ArtistController(MusicDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ArtistListViewModel viewModel = new ArtistListViewModel();
            viewModel.Artists = await _context.Artists.OrderBy(x => x.Name).ToListAsync();
            return View(viewModel);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArtistViewModel viewModel)
        {
            var artistindb = await _context.Artists.Where(x => x.Name == viewModel.Name).FirstOrDefaultAsync();

            if (artistindb != null)
            {
                ModelState.AddModelError("DuplicateError", "Error, you tried to make a duplicate!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(new Artist()
                {
                    Name = viewModel.Name,
                    //Members = viewModel.Members,
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

            var artist = await _context.Artists.FirstOrDefaultAsync(x => x.ID == id);
            if (artist == null)
            {
                return NotFound();
            }

            DeleteArtistViewModel viewModel = new DeleteArtistViewModel()
            {
                ArtistID = artist.ID,
                Name = artist.Name,
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            ICollection<SongArtist> songArtists = await _context.SongArtists.Include(x => x.Song)
                                                                  .Include(x => x.Song.Album)
                                                                  .Include(x => x.Artist)
                                                                  .Where(x => x.ArtistID == id)
                                                                  .ToListAsync();
            if (songArtists.Count() == 0)
            {

                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Error", "First delete all the songs of this artist before deleting the artist!");
                DeleteArtistViewModel viewModel = new DeleteArtistViewModel()
                {
                    ArtistID = artist.ID,
                    Name = artist.Name,
                };

                return View(viewModel);
            }

        }
        #endregion

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            Artist artist = await _context.Artists.Where(x => x.ID == id).FirstOrDefaultAsync();

            ICollection<SongArtist> songsOfArtist = await _context.SongArtists.Include(x => x.Song)
                                                                              .Include(x => x.Song.Album)
                                                                              .Include(x => x.Artist)
                                                                              .Where(x => x.ArtistID == id)
                                                                              .ToListAsync();

            ICollection<SongArtist> songArtists = await _context.SongArtists.Include(x => x.Artist)
                                                                            .ToListAsync();

            if (artist != null)
            {
                ArtistDetailsViewModel viewModel = new ArtistDetailsViewModel()
                {
                    Name = artist.Name,
                    //Members = artist.Members,
                    SongsOfArtist = songsOfArtist,
                    SongArtists = songArtists,
                };
                return View(viewModel);
            }
            else
            {
                ArtistListViewModel viewModel = new ArtistListViewModel()
                {
                    Artists = _context.Artists.ToList()
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

            var artist = await _context.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            EditArtistViewModel viewModel = new EditArtistViewModel()
            {
                ArtistID = artist.ID,
                Name = artist.Name,
                //Members = artist.Members,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditArtistViewModel viewModel)
        {
            if (id != viewModel.ArtistID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Artist artist = new Artist()
                    {
                        ID = viewModel.ArtistID,
                        Name = viewModel.Name,
                        //Members = viewModel.Members,
                    };
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Artists.Any(x => x.ID == viewModel.ArtistID))
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
        public async Task<IActionResult> Search(ArtistListViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.ArtistSearch))
            {
                viewModel.Artists = await _context.Artists.Where(x => x.Name.Contains(viewModel.ArtistSearch)/* ||
                                                    x.Members.Contains(viewModel.ArtistSearch)*/).ToListAsync();
            }
            else
            {
                viewModel.Artists = await _context.Artists.ToListAsync();
            }
            return View("Index", viewModel);
        }

    }
}
