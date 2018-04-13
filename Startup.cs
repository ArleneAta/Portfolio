using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Data;
using Portfolio.Models;
using Portfolio.Services;
using Stripe;

namespace Portfolio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //commented out email verification for testing
            //services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            //{
            //    config.SignIn.RequireConfirmedEmail = true;
            //});

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            //Enable lockout features
            services.Configure<IdentityOptions>(options => {
                //// Password settings if you want to ensure password strength.
                //options.Password.RequireDigit           = true;
                //options.Password.RequiredLength         = 8;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase       = true;
                //options.Password.RequireLowercase       = false;
                //options.Password.RequiredUniqueChars    = 6;

                // Lockout settings (Freeze 1 minute only to make testing easier)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3; // Lock after 3 consec failed logins
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

            });

            services.AddMvc();

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
