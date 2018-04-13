using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Data;
using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Repositories
{
    public class UserRepo
    {
        ApplicationDbContext _context;
        IServiceProvider serviceProvider;

        public UserRepo(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            this._context = context;
            this.serviceProvider = serviceProvider;
        }

        // Get all users in the database.
        public IEnumerable<UserVM> All()
        {
            var users = _context.Users.Select(u => new UserVM()
            {
                Email = u.Email
            });
            return users;
        }
        
        public List<UserRoleVM> GetAllUsers()
        {
            List<UserRoleVM> userDeets = new List<UserRoleVM>();
            UserRoleRepo userRoleRepo = new UserRoleRepo(serviceProvider, _context);
            var users = _context.Users;
            foreach (var u in users)
            {
                UserRoleVM user = new UserRoleVM
                {
                    Email = u.Email,
                    Role = userRoleRepo.GetUserRole(u.Email)
                };
                userDeets.Add(user);
            }
            return userDeets;
        }

    }

}
