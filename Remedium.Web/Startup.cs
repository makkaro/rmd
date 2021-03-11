using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper(typeof(ApplicationUserProfile));

            services.AddControllersWithViews();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("RequireModerator", policy => policy.RequireRole("Administrator", "Moderator"));
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
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            
            if (env.IsDevelopment())
            {
                AddDefaultRoles(serviceProvider);
            }
        }
        
        private static async void AddDefaultRoles(IServiceProvider serviceProvider)
        {
            using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            if (roleManager.Roles.Any() || await userManager.FindByNameAsync("admin") is null) return;
            await new[] {"Administrator", "Moderator"}.ForEachAsync(async role =>
            {
                await roleManager.CreateAsync(new(role));
            });
            
            var admin = await userManager.FindByNameAsync("admin");
            if (admin is not null)
            {
                await userManager.AddToRoleAsync(admin, "Administrator");
            }
            var moderator = await userManager.FindByNameAsync("moderator");
            if (moderator is not null)
            {
                await userManager.AddToRoleAsync(moderator, "Moderator");
            }
        }
    }
}