using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;
using VBeat.Models.DatabaseContext;

namespace VBeat.Models
{
    public class ArtistDbContext : MainDbContext
    {
        public DbSet<ArtistModel> Artists { get; set; }
        public DbSet<SongModel> Songs { get; set; }
        public DbSet<ShowModel> Shows { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtistSongModel>().HasKey(t => new { t.SongId, t.UserId });
            modelBuilder.Entity<ArtistShowModel>().HasKey(t => new { t.ShowId, t.UserId });
        }
    }
}
