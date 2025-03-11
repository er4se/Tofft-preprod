using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.Models;

namespace Tofft_preprod.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Tofft_preprod.Models.User> BaseUsers { get; set; }
        public DbSet<Tofft_preprod.Models.Board> Boards { get; set; }
        public DbSet<Tofft_preprod.Models.Mission> Missions { get; set; }
        public DbSet<Tofft_preprod.Models.Report> Reports { get; set; }
        public DbSet<Tofft_preprod.Models.ImageData> Images { get; set; }
        public DbSet<Tofft_preprod.Models.FileData> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Board>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
