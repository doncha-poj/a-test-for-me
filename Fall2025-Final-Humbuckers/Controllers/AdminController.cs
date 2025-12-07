using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fall2025_Final_Humbuckers.Models;
using Fall2025_Final_Humbuckers.Models.ViewModels;
using Fall2025_Final_Humbuckers.Data;

// Admin can manage users
// Delete and Edit
namespace Fall2025_Final_Humbuckers.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserManagementViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserManagementViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            return View(userViewModels);
        }

        // GET: Admin/EditUser/5
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var viewModel = new EditUserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                CurrentRoles = userRoles.ToList(),
                AllRoles = allRoles.Select(r => r.Name).ToList()
            };

            return View(viewModel);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, List<string> selectedRoles)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Get current roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Remove all current roles
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return View();
            }

            // Add selected roles
            if (selectedRoles != null && selectedRoles.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to add user roles");
                    return View();
                }
            }

            return RedirectToAction(nameof(Users));
        }

        // GET: Admin/DeleteUser/5
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserManagementViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            };

            return View(viewModel);
        }

        // POST: Admin/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Prevent deleting yourself
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == user.Id)
            {
                TempData["Error"] = "You cannot delete your own account!";
                return RedirectToAction(nameof(Users));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = $"User {user.Email} has been deleted.";
                return RedirectToAction(nameof(Users));
            }

            TempData["Error"] = "Failed to delete user.";
            return RedirectToAction(nameof(Users));
        }

        public IActionResult Metrics()
        {
            var globalTracks = _context.GlobalTracks.ToList();
            var users = _context.Users.ToList();
            var tracks = _context.FavoriteTracks.ToList(); 

            var favorites = _context.FavoriteTracks.ToList();

            var topArtist = (from f in _context.FavoriteTracks
                             join g in _context.GlobalTracks
                             on f.SpotifyId equals g.SpotifyId
                             select g.Artist)
                           .GroupBy(name => name)
                           .OrderByDescending(g => g.Count())
                           .Select(g => g.Key)
                           .FirstOrDefault() ?? "N/A";

            var viewModel = new AdminReportViewModel
            {
                TotalUsers = users.Count(),
                TotalFavoriteTracks = favorites.Count(),
                TopArtist = topArtist, // Use the variable we calculated above
                AverageTracksPerUser = users.Count > 0 ? favorites.Count / users.Count : 0
            };

            return View(viewModel);
        }
    }
}
