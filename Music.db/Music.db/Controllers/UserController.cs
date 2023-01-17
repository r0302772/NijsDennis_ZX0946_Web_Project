using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Music.db.Areas.Identity.Data;
using Music.db.Data;
using Music.db.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Controllers
{
    //Login als admin
    //Username = admin  Password = adminn

    //Login als user
    //Username = user   Password = userrr
    //[Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private UserManager<CustomUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            UserListViewModel viewModel = new UserListViewModel()
            {
                Users = _userManager.Users.ToList()
            };

            return View(viewModel);
        }

        #region Details
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomUser user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();

            if (user != null)
            {
                UserDetailsViewModel viewModel = new UserDetailsViewModel()
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    //Role = user.Role
                };

                return View(viewModel);
            }
            else
            {
                UserListViewModel viewModel = new UserListViewModel()
                {
                    Users = _userManager.Users.ToList()
                };
                return View("Index", viewModel);
            }
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            DeleteUserViewModel viewModel = new DeleteUserViewModel()
            {
                UserID = user.Id,
                UserName = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            CustomUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View("Index", _userManager.Users.ToList());
        }
        #endregion

        #region GrantPermissions
        public IActionResult GrantPermissions()
        {
            GrantPermissionsViewModel viewModel = new GrantPermissionsViewModel()
            {
                Users = new SelectList(_userManager.Users.ToList(), "Id", "UserName"),
                Roles = new SelectList(_roleManager.Roles.ToList(), "Id", "Name")
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("GrantPermissions")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GrantPermissionsConfirmed(GrantPermissionsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                CustomUser user = await _userManager.FindByIdAsync(viewModel.UserID);
                IdentityRole role = await _roleManager.FindByIdAsync(viewModel.RoleID);

                if (user != null && role != null)
                {
                    IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User or role not found.");
                }
            }
            return View(viewModel);
        }
        #endregion

        public IActionResult Search(UserListViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.UserSearch))
            {
                viewModel.Users = _userManager.Users.Where(x => x.Firstname.Contains(viewModel.UserSearch) ||
                                                            x.Lastname.Contains(viewModel.UserSearch) ||
                                                            x.UserName.Contains(viewModel.UserSearch)).ToList();
            }
            else
            {
                viewModel.Users = _userManager.Users.ToList();
            }
            return View("Index", viewModel);
        }
    }
}
