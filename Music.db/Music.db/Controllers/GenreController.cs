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
    public class GenreController : Controller
    {
        private readonly MusicDbContext _context;

        public GenreController(MusicDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            GenreListViewModel viewModel = new GenreListViewModel();
            viewModel.Genres = await _context.Genres.OrderBy(x => x.Name).ToListAsync();
            return View(viewModel);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreViewModel viewModel)
        {
            var genreindb = await _context.Genres.Where(x => x.Name == viewModel.Name).FirstOrDefaultAsync();

            if (genreindb != null)
            {
                ModelState.AddModelError("DuplicateError", "Error, you tried to make a duplicate!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(new Genre()
                {
                    Name = viewModel.Name,
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

            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.ID == id);
            if (genre == null)
            {
                return NotFound();
            }

            DeleteGenreViewModel viewModel = new DeleteGenreViewModel()
            {
                GenreID = genre.ID,
                Name = genre.Name,
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            ICollection<SongArtist> songArtists = await _context.SongArtists.Include(x => x.Song)
                                                      .Include(x => x.Song.Genre)
                                                      .Where(x => x.Song.GenreID == id)
                                                      .ToListAsync();
            if (songArtists.Count() == 0)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Error", "First delete all the songs with this genre before deleting this genre!");
                DeleteGenreViewModel viewModel = new DeleteGenreViewModel()
                {
                    GenreID = genre.ID,
                    Name = genre.Name,
                };

                return View(viewModel);
            }
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            EditGenreViewModel viewModel = new EditGenreViewModel()
            {
                GenreID = genre.ID,
                Name = genre.Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditGenreViewModel viewModel)
        {
            if (id != viewModel.GenreID)
            {
                return NotFound();
            }

            var genreindb = await _context.Genres.Where(x => x.Name == viewModel.Name).FirstOrDefaultAsync();

            if (genreindb != null)
            {
                ModelState.AddModelError("DuplicateError", "Error, you tried to make a duplicate!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Genre genre = new Genre()
                    {
                        ID = viewModel.GenreID,
                        Name = viewModel.Name,
                    };
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Genres.Any(x => x.ID == viewModel.GenreID))
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
        public async Task<IActionResult> Search(GenreListViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.GenreSearch))
            {
                viewModel.Genres = await _context.Genres.Where(x => x.Name.Contains(viewModel.GenreSearch)).ToListAsync();
            }
            else
            {
                viewModel.Genres = await _context.Genres.ToListAsync();
            }
            return View("Index", viewModel);
        }

    }
}
