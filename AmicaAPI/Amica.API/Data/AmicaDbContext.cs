using Amica.API.Data.Models;
using Amica.API.WebServer.Data.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Amica.API.Data {
    public class AmicaDbContext : IdentityDbContext<Account> {
        //public DbSet<Account> Accounts { get; set; }
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;

        public AmicaDbContext(DbContextOptions options) : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder b) {
            base.OnModelCreating(b);

            // setup date attachment when post creating
            b.Entity<Post>().
                Property(p => p.DateCreated).
                HasDefaultValueSql("GETUTCDATE()");

            // setup deleting refered images(not at blob server, just in db) when post is deleted
            b.Entity<Post>().
                HasMany(p => p.Images).
                WithOne().
                OnDelete(DeleteBehavior.Cascade);

#pragma warning disable CS0618 // Ignore that
            b.Entity<Account>().Ignore(c => c.UserName);
#pragma warning restore CS0618 // Ignore that
            b.Entity<Account>().Ignore(c => c.NormalizedUserName);
            b.Entity<Account>().ToTable("Accounts");

            b.Entity<Profile>().
                HasMany<Profile>(p => p.Followings).
                WithMany(p => p.Followers).UsingEntity(j => {
                    j.ToTable("Followers");
                });
        }

        public async Task<bool> RebuildDatabaseAsync() {
            bool success = true;
            success &= await Database.EnsureDeletedAsync();
            success &= await Database.EnsureCreatedAsync();
            return success;
        }
    }
}
