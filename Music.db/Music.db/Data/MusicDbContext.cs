using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Music.db.Areas.Identity.Data;
using Music.db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.db.Data
{
    public class MusicDbContext : IdentityDbContext<CustomUser>
    {
        public MusicDbContext(DbContextOptions<MusicDbContext> options)
            : base(options)
        {

        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        //public DbSet<AlbumArtist> AlbumArtists { get; set; }
        public DbSet<SongArtist> SongArtists { get; set; }
        public DbSet<UserCollection> UserCollection { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Album>().ToTable("Album");
            modelBuilder.Entity<Album>().Property(a => a.AlbumTitle).IsRequired();

            modelBuilder.Entity<Artist>().ToTable("Artist");
            modelBuilder.Entity<Artist>().Property(a => a.Name).IsRequired();
            //modelBuilder.Entity<Artist>().Property(b => b.Members).IsRequired();

            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<Genre>().Property(a => a.Name).IsRequired();

            modelBuilder.Entity<Song>().ToTable("Song");
            modelBuilder.Entity<Song>().Property(a => a.SongTitle).IsRequired();
            modelBuilder.Entity<Song>().Property(b => b.BPM).IsRequired();
            modelBuilder.Entity<Song>().Property(c => c.Key).IsRequired();
            modelBuilder.Entity<Song>().Property(d => d.ReleaseDate).IsRequired();

            modelBuilder.Entity<UserCollection>()
                .HasOne(a => a.Song)
                .WithMany(b => b.UserCollectionSongs)
                .HasForeignKey(c => c.SongID)
                .IsRequired();
            modelBuilder.Entity<UserCollection>()
                .HasOne(a => a.User)
                .WithMany(b => b.UserCollectionSongs)
                .HasForeignKey(c => c.UserID)
                .IsRequired();

            //modelBuilder.Entity<AlbumArtist>().HasKey(a => new { a.AlbumID, a.ArtistID });
            //modelBuilder.Entity<AlbumArtist>()
            //    .HasOne(a => a.Album)
            //    .WithMany(b => b.AlbumArtists)
            //    .HasForeignKey(c => c.AlbumID)
            //    .IsRequired();
            //modelBuilder.Entity<AlbumArtist>()
            //    .HasOne(a => a.Artist)
            //    .WithMany(b => b.AlbumArtists)
            //    .HasForeignKey(c => c.ArtistID)
            //    .IsRequired();

            //modelBuilder.Entity<SongArtist>().HasKey(a => new { a.SongID, a.ArtistID });
            modelBuilder.Entity<SongArtist>()
                .HasOne(a => a.Song)
                .WithMany(b => b.SongArtists)
                .HasForeignKey(c => c.SongID)
                .IsRequired();
            modelBuilder.Entity<SongArtist>()
                .HasOne(a => a.Artist)
                .WithMany(b => b.SongArtists)
                .HasForeignKey(c => c.ArtistID)
                .IsRequired();
        }
    }
}
