using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoHosting.Services.Entities;

namespace VideoHostind.DataBase
{
    public class DataBaseContext : IdentityDbContext<UserLogin>
    {
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Comentary> Comentaries { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
             : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server = .\SQLEXPRESS; Database = VideoHostingCore; Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserUser>()
                .HasKey(x => new { x.SubscriberId, x.SubscripterId });
            builder.Entity<VideoUser>()
                .HasKey(x => new { x.UserProfileId, x.VideoId });

            //builder.Entity<UserLogin>()
            //    .HasKey(x => new {x.Id})
            //    .IsClustered();

            builder.Entity<UserLogin>()
                .HasOne(x => x.UserProfile)
                .WithOne(x => x.UserLogin)
                .HasForeignKey<UserLogin>(x => x.Id);

            builder.Entity<UserProfile>()
                .HasMany(x => x.Videos)
                .WithOne(x => x.UserProfile);

            builder.Entity<UserProfile>()
                .HasMany(x => x.Comentaries)
                .WithOne(x => x.UserProfile);

            builder.Entity<UserProfile>()
                .HasMany(x => x.Subscribers)
                .WithOne(x => x.Subscripter)
                .HasForeignKey(x => x.SubscripterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserProfile>()
                .HasMany(x => x.Subscriptions)
                .WithOne(x => x.Subscriber)
                .HasForeignKey(x => x.SubscriberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserProfile>()
                .HasOne(x => x.Country)
                .WithMany(x => x.Users);

            builder.Entity<VideoUser>()
                .HasOne(x => x.UserProfile)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.UserProfileId);
            builder.Entity<VideoUser>()
                .HasOne(x => x.Video)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.VideoId);

            //builder.Entity<VideoUser>()
            //    .HasOne(x => x.UserProfile)
            //    .WithMany(x => x.Dislikes)
            //    .HasForeignKey(x => x.UserProfileId);
            //builder.Entity<VideoUser>()
            //    .HasOne(x => x.Video)
            //    .WithMany(x => x.Dislikes)
            //    .HasForeignKey(x => x.VideoId);

            //builder.Entity<UserProfile>()
            //    .HasMany(x => x.Dislikes)
            //    .WithOne(x => x.UserProfile)
            //    .HasForeignKey(x => x.UserId); 

            //builder.Entity<UserProfile>()
            //    .HasMany(x => x.Dislikes)
            //    .WithOne(x => x.UserProfile)
            //    .HasForeignKey(x => x.UserId); 

            builder.Entity<Video>()
                .HasMany(x => x.Comentaries)
                .WithOne(x => x.Video);

            //builder.Entity<Video>()
            //   .HasMany(x => x.Dislikes)
            //   .WithOne(x => x.Video)
            //   .HasForeignKey(x=>x.VideoId);

            //builder.Entity<Video>()
            //   .HasMany(x => x.Dislikes)
            //   .WithOne(x => x.Video)
            //   .HasForeignKey(x => x.VideoId); 

            base.OnModelCreating(builder);
        }
    }
}
