using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Remedium.Web.Data;
using Remedium.Web.Data.Entities;
using Remedium.Web.Data.Profiles;
using Remedium.Web.Utilities;

namespace Remedium.Web
{
    public sealed class Startup
    {
        private readonly IConfiguration _config;

        
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        
        
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IRepository, Repository>();

            services.AddAutoMapper(typeof(ApplicationUserProfile));

            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministrator", policy => policy.RequireRole("Administrator"));
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Blog}/{action=Index}/{id?}");
            });

            
            if (env.IsDevelopment() && _config.GetValue<Boolean>("Flags:Setup"))
            {
                AddDefaultUsersWithRoles(serviceProvider);
            }
        }

        

        #region *** Setup ***
        
        private static async void AddDefaultUsersWithRoles(IServiceProvider serviceProvider)
        {
            var admin = new ApplicationUser {UserName = "admin", Email = "admin@remedium.com"};
            var mod = new ApplicationUser {UserName = "mod", Email = "mod@remedium.com"};
            var user = new ApplicationUser {UserName = "user", Email = "user@remedium.com"};
            var users = new[] {admin, mod, user};
            
            using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await new[] {"Administrator", "Moderator"}.ForEachAsync(async role =>
            {
                await roleManager.CreateAsync(new(role));
            });

            using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await users.ForEachAsync(async applicationUser =>
            {
                await userManager.CreateAsync(applicationUser, applicationUser.UserName + "1");
            });
            await userManager.AddToRoleAsync(admin, "Administrator");
            await userManager.AddToRoleAsync(mod, "Moderator");
        }

        #endregion
    }
}