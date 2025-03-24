using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
//using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;
using Tofft_preprod.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Tofft_preprod
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IBoardAuthService, BoardAuthService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireUser", policy => policy.RequireRole("User"));

                CreateRolePolicy(options, "BoardAdmin", BoardRole.Admin);
                CreateRolePolicy(options, "BoardModerator", BoardRole.Moderator);
                CreateRolePolicy(options, "BoardLead", BoardRole.Lead);
                CreateRolePolicy(options, "BoardMember", BoardRole.Memeber);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"); //pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "invite",
                pattern: "invite/{action}/{boardId?}",
                defaults: new { controller = "Invite" });

            //app.MapControllerRoute(
            //    name: "invite",
            //    pattern: "invite/{token}",
            //    defaults: new { controller = "Invite", action = "AcceptInvite"});

            app.Run();
        }

        private static void CreateRolePolicy(AuthorizationOptions options, string policyName, BoardRole requiredRole)
        {
            options.AddPolicy(policyName, policy =>
                policy.RequireAssertion(async context =>
                {
                    var httpContext = context.Resource as DefaultHttpContext;
                    if (httpContext == null) return false;
            
                    var boardId = httpContext.Request.RouteValues["boardId"] as string;
                    boardId = boardId ?? httpContext.Request.RouteValues["id"] as string;
            
                    var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
                    if (string.IsNullOrEmpty(userId) || (string.IsNullOrEmpty(boardId)))
                        return false;
            
                    var authService = httpContext.RequestServices.GetRequiredService<IBoardAuthService>();
            
                    return await authService.HasRoleAsync(boardId, userId, requiredRole);
                }));
        }
    }
}
