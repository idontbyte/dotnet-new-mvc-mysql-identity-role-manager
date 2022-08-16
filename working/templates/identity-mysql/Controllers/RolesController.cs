using identity_mysql.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace identity_mysql.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        //[Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            // Populate Dropdown Lists
            var rolelist = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = rolelist;

            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;

            ViewBag.Message = "";

            return View();
        }


        // GET: /Roles/Create
        //[Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(collection["RoleName"]));
            ViewBag.Message = "Role created successfully !";
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Administrator")]
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name == RoleName).Single();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult> RoleAddToUser(string UserName, string RoleName)
        {
            var user = context.Users.Where(u => u.UserName == UserName).Single();

            await userManager.AddToRoleAsync(user, RoleName);

            ViewBag.Message = "Role created successfully !";

            RepopulateDropdownLists();

            return View("Index");
        }

        private void RepopulateDropdownLists()
        {
            var rolelist = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = rolelist;
            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu =>
            new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var user = context.Users.Where(u => u.UserName == UserName).Single();

                ViewBag.RolesForThisUser = await userManager.GetRolesAsync(user);

                RepopulateDropdownLists();
                ViewBag.Message = "Roles retrieved successfully !";
            }

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteRoleForUser(string UserName, string RoleName)
        {
            var user = context.Users.Where(u => u.UserName == UserName).Single();

            if (await userManager.IsInRoleAsync(user, RoleName))
            {
                await userManager.RemoveFromRoleAsync(user, RoleName);
                ViewBag.Message = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.Message = "This user doesn't belong to selected role.";
            }

            RepopulateDropdownLists();

            return View("Index");
        }
    }
}