using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.Models;

namespace Tofft_preprod.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Tofft_preprod.Models.UserToBoard> UserToBoards { get; set; }
        public DbSet<Tofft_preprod.Models.Board> Boards { get; set; }
        public DbSet<Tofft_preprod.Models.Mission> Missions { get; set; }
        public DbSet<Tofft_preprod.Models.Report> Reports { get; set; }
        public DbSet<Tofft_preprod.Models.ImageData> Images { get; set; }
        public DbSet<Tofft_preprod.Models.FileData> Files { get; set; }
        public DbSet<Tofft_preprod.Models.InviteLink> InviteLinks { get; set; }
        public DbSet<Tofft_preprod.Models.JoinRequest> JoinRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Board>()
            //    .Property(x => x.Id)
            //    .ValueGeneratedOnAdd();

            builder.Entity<UserToBoard>()
                .HasKey(ub => new {ub.UserId, ub.BoardId});

            builder.Entity<UserToBoard>()
                .HasOne(ub => ub.User)
                .WithMany(b => b.UserToBoards)
                .HasForeignKey(ub => ub.UserId);

            builder.Entity<UserToBoard>()
                .HasOne(ub => ub.Board)
                .WithMany(b => b.UserToBoards)
                .HasForeignKey(ub => ub.BoardId);

            builder.Entity<Mission>()
                .HasOne(m => m.Board)
                .WithMany(b => b.Missions)
                .HasForeignKey(m => m.BoardId);

            builder.Entity<InviteLink>()
                .HasIndex(i => i.Token)
                .IsUnique();

            builder.Entity<JoinRequest>()
                .HasKey(r => new {r.UserId, r.BoardId});
        }
    }
}
