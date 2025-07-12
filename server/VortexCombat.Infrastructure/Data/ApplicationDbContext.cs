using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;

namespace VortexCombat.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutMaster> WorkoutMasters { get; set; }
        public DbSet<WorkoutStudent> WorkoutStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutMaster>()
                .HasKey(wm => new { wm.WorkoutId, wm.MasterId });

            modelBuilder.Entity<WorkoutMaster>()
                .HasOne(wm => wm.Workout)
                .WithMany(w => w.WorkoutMasters)
                .HasForeignKey(wm => wm.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutMaster>()
                .HasOne(wm => wm.Master)
                .WithMany(m => m.WorkoutMasters)
                .HasForeignKey(wm => wm.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkoutStudent>()
                .HasKey(ws => new { ws.WorkoutId, ws.StudentId });

            modelBuilder.Entity<WorkoutStudent>()
                .HasOne(ws => ws.Workout)
                .WithMany(w => w.WorkoutStudents)
                .HasForeignKey(ws => ws.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutStudent>()
                .HasOne(ws => ws.Student)
                .WithMany(s => s.WorkoutStudents)
                .HasForeignKey(ws => ws.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>().OwnsOne(u => u.Address);

            modelBuilder.Entity<ApplicationUser>().OwnsOne(u => u.Belt);
        }
    }
}