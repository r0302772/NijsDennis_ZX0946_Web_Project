using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Music.db.Areas.Identity.Data;
using Music.db.Data;
using Music.db.ViewModels;
using System;
using System.Collections.Generic;
using Music.db.Models;

//under construction

namespace Music.db.Areas.Identity.Pages.Account.Manage
{
    public class MyCollectionModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly ILogger<MyCollectionModel> _logger;
        private readonly MusicDbContext _context;

        public MyCollectionModel(
            UserManager<CustomUser> userManager,
            ILogger<MyCollectionModel> logger, MusicDbContext context
            )
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        //[TempData]
        //public virtual ICollection<SongArtist> SongArtists { get; set; }
        //[TempData]
        //public virtual ICollection<UserCollection> UserCollectionSongs { get; set; }

        public async Task<IActionResult> OnGet()
        {
            SongListViewModel viewModel = new SongListViewModel();  
            var user = await _userManager.GetUserAsync(User);
            viewModel.UserCollectionSongs = await _context.UserCollection.Where(x => x.UserID == user.Id).ToListAsync();
            viewModel.SongArtists = await _context.SongArtists.Include(x => x.Artist).ToListAsync();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}