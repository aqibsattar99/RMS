using Microsoft.EntityFrameworkCore;
using System.Drawing;
using RMS.Models;

namespace RMS.Models
{
    public partial class dbRMSContext : DbContext
    {


        public dbRMSContext(DbContextOptions<dbRMSContext> options)
            : base(options)
        {
        }
        public DbSet<Branch> Branch { get; set; } = default!;
        public DbSet<Eqptname> Eqptname { get; set; } = default!;
        public DbSet<Eqptissue> Eqptissue { get; set; } = default!;
        public DbSet<Eqptstore> Eqptstore { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Eqptstore>()
                .HasOne(e => e.Eqptname)
                .WithMany()  // Adjust this as needed
                .HasForeignKey(e => e.eqptid);
        }

        //public virtual DbSet<Color> Colors { get; set; } = null!;
    }
}