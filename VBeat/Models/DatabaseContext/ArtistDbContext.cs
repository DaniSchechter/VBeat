using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;

namespace VBeat.Models
{
    public class ArtistDbContext : DbContext
    {
        public DbSet<ArtistModel> Artists { get; set; }
        public DbSet<SongModel> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArtistSongModel>().HasKey(t => new { t.SongId, t.UserId });
        }
    }
}
