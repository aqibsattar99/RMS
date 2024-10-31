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
        public DbSet<Eqpttype> Eqpttype { get; set; } = default!;
        public DbSet<Eqptissue> Eqptissue { get; set; } = default!;
        public DbSet<Eqptstore> Eqptstore { get; set; } = default!;
        public DbSet<Tasks> Tasks { get; set; } = default!;
        public DbSet<BranchUsers> BranchUsers { get; set; } = default!;
        public DbSet<Eqptcondition> Eqptcondition { get; set; } = default!;
        public DbSet<Eqptissuehistory> Eqptissuehistory { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Eqptstore>()
                .HasOne(e => e.Eqpttype)
                .WithMany()  // Adjust this as needed
                .HasForeignKey(e => e.Eqptid);



            modelBuilder.Entity<Tasks>()
       .HasOne(t => t.BranchUsers)
       .WithMany()
       .HasForeignKey(t => t.Assignedid);

            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Branch)
                .WithMany()
                .HasForeignKey(t => t.Branchid);



        }

        //public virtual DbSet<Color> Colors { get; set; } = null!;
    }
}