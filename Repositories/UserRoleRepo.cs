using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Controllers;
using Portfolio.Data;
using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Repositories
{
    public class UserRoleRepo
    {
        IServiceProvider serviceProvider;
        ApplicationDbContext context;

        public UserRoleRepo(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            this.serviceProvider = serviceProvider;
            this.context = context;
        }

        // Assign a role to a user.
        public async Task<bool> AddUserRole(string email, string roleName)
        {
            var UserManager = serviceProvider
                                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                await UserManager.AddToRoleAsync(user, roleName);
            }
            return true;
        }

        // Remove role from a user.
        public async Task<bool> RemoveUserRole(string email, string roleName)
        {
            var UserManager = serviceProvider
                                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                await UserManager.RemoveFromRoleAsync(user, roleName);
            }
            return true;
        }
        public string GetUserRole(string id)
        {
            var role = context.UserRoles.Where(r => r.UserId == id).FirstOrDefault();
            return role.RoleId;
        }

        public ApplicationUser GetUser(string email)
        {
            var user = context.Users.Where(r => r.Email == email).FirstOrDefault();
            return user;
        }
        // Get all roles of a specific user.
        public string GetUserRoles(string email)
        {
            var UserManager = serviceProvider
                                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = GetUser(email);
            var role = GetUserRole(user.Id);
            //var roles = await UserManager.GetRolesAsync(user);
            RoleVM roleVMObjects = new RoleVM();

            roleVMObjects = new RoleVM() { Id = user.Id, RoleName = role };

            return roleVMObjects.RoleName;
        }
        //// Get all roles of a specific user.
        public async Task<IEnumerable<RoleVM>> GettheUserRoles(string email)
        {
            var UserManager = serviceProvider
                                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            var roles = await UserManager.GetRolesAsync(user);
            List<RoleVM> roleVMObjects = new List<RoleVM>();
            foreach (var item in roles)
            {
                roleVMObjects.Add(new RoleVM() { Id = item, RoleName = item });
            }
            return roleVMObjects;
        }

        public List<UserRoleVM> GetAllUsers()
        {
            List<UserRoleVM> list = new List<UserRoleVM>();
            UserRoleRepo urRepo = new UserRoleRepo(serviceProvider, context);
            var users = context.Users;
            foreach(var u in users)
            {
                UserRoleVM user = new UserRoleVM
                {
                    Email = u.Email,
                    Role = urRepo.GetUserRoles(u.Email)
                };
                list.Add(user);
            }

            return list;
        }
    }

}
