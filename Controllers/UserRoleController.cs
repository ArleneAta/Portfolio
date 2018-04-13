using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;
using Portfolio.Repositories;

namespace Portfolio.Controllers
{
    // This annotation can be used at the class or method level.
    // The annotation could include a comma separated list or different
    // roles.
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private ApplicationDbContext _context;
        private IServiceProvider _serviceProvider;

        public UserRoleController(ApplicationDbContext context,
                                    IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public ActionResult Index()
        {
            UserRepo userRepo = new UserRepo( _context, _serviceProvider);
            var users = userRepo.All();
            return View(users);
        }
        //public async Task<ActionResult> Delete(string userName, string roleName)
        //{

        //    UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider, _context);
        //    var roles = await userRoleRepo.RemoveUserRole(userName, roleName);
        //    ViewBag.UserName = userName;
        //    return View(roles);
        //}
        // GET: Employee/Delete/  
        //public async Task<IActionResult> Delete(string roleName, string userName)
        //{
        //    if (userName == null)
        //    {
        //        return NotFound();
        //    }
        //    UserRoleRepo urRepo = new UserRoleRepo(_serviceProvider, _context);
        //    var users = await _context.Roles.SingleOrDefaultAsync(m => m.Id == urRepo.GetUserRole(m.Id));
        //    return View(users);
        //}

        //// POST: Employee/Delete/
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string roleName, string userName)
        //{
        //    UserRoleRepo urRepo = new UserRoleRepo(_serviceProvider, _context);
        //    var users = await urRepo.RemoveUserRole(roleName, userName);
          
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        // Show all roles for a specific user.
        public async Task<IActionResult>  Detail(string userName)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider, _context);
            var roles = await userRoleRepo.GettheUserRoles(userName);
            ViewBag.UserName = userName;
            return View(roles);
        }

        public IActionResult List()
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider, _context);
            var users = userRoleRepo.GetAllUsers();


            return View(users);
        }
        // Present user with ability to assign roles to a user.
        // It gives two drop downs - the first contains the user names with
        // the requested user selected. The second drop down contains all
        // possible roles.
        public ActionResult Assign(string userName)
        {
            // Store the email address of the Identity user
            // which is their user name.
            ViewBag.SelectedUser = userName;

            // Build SelectList with role data and store in ViewBag.
            RoleRepo roleRepo = new RoleRepo(_context);
            var roles = roleRepo.GetAllRoles().ToList();

            // There may be a better way but I have always found using the 
            // .NET dropdown lists to be a challenge. Here is a way to make 
            // it work if you can get the data in the proper format. 

            // 1. Preparation for 'Roles' drop down.
            // a) Build a list of SelectListItem objects which have 'Value' and 
            // 'Text' properties. 
            var preRoleList = roles.Select(r =>
                new SelectListItem { Value = r.RoleName, Text = r.RoleName })
                   .ToList();
            // b) Store the SelectListItem objects in a SelectList object 
            // with 'Value' and 'Text' properties set specifically.
            var roleList = new SelectList(preRoleList, "Value", "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.RoleSelectList = roleList;

            // 2. Preparation for 'Users' drop down list. 
            // a) Build a list of SelectListItem objects which have 'Value' and 
            // 'Text' properties.
            var userList = _context.Users.ToList();

            // b) Store the SelectListItem objects in a SelectList object 
            // with 'Value' and 'Text' properties set specifically.
            var preUserList = userList.Select(u => new SelectListItem
            { Value = u.Email, Text = u.Email }).ToList();
            SelectList userSelectList = new SelectList(preUserList, "Value", "Text");

            // c) Store the SelectList in a ViewBag.
            ViewBag.UserSelectList = userSelectList;
            return View();
        }

        // Assigns role to user.
        [HttpPost]
        public async Task<IActionResult> Assign(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_serviceProvider, _context);

            if (ModelState.IsValid)
            {
                var addUR = await userRoleRepo.AddUserRole(userRoleVM.Email,
                                                            userRoleVM.Role);
            }
            try
            {
                return RedirectToAction("Detail", "UserRole",
                       new { userName = userRoleVM.Email });
            }
            catch
            {
                return View();
            }
        }
    }

}