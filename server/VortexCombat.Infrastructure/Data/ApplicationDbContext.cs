using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Common;
using VortexCombat.Infrastructure.Identity;

namespace VortexCombat.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutMaster> WorkoutMasters { get; set; }
        public DbSet<WorkoutStudent> WorkoutStudents { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercise { get; set; }
        public DbSet<StudentWorkoutExercise> StudentWorkoutExercise { get; set; }

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

            //modelBuilder.Entity<ApplicationUser>().OwnsOne(u => u.Address);

            //modelBuilder.Entity<ApplicationUser>().OwnsOne(u => u.Belt);

            modelBuilder.Entity<WorkoutExercise>()
                .HasKey(we => new { we.WorkoutId, we.ExerciseId });

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentWorkoutExercise>()
                .HasKey(swe => new { swe.WorkoutId, swe.StudentId, swe.ExerciseId });

            modelBuilder.Entity<StudentWorkoutExercise>()
                .HasOne(swe => swe.Workout)
                .WithMany()
                .HasForeignKey(swe => swe.WorkoutId);

            modelBuilder.Entity<StudentWorkoutExercise>()
                .HasOne(swe => swe.Student)
                .WithMany()
                .HasForeignKey(swe => swe.StudentId);

            modelBuilder.Entity<StudentWorkoutExercise>()
                .HasOne(swe => swe.Exercise)
                .WithMany()
                .HasForeignKey(swe => swe.ExerciseId);

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.Id)
                 .HasConversion(v => v.Value, v => new UserId(v))
                 .ValueGeneratedNever();

                e.Property(u => u.Name);
                e.Property(u => u.EGender);
                e.Property(u => u.Birthday);
                e.Property(u => u.Height);
                e.Property(u => u.Weight);
                e.Property(u => u.Nif);

                e.OwnsOne(u => u.Belt, b =>
                {
                    b.Property(p => p.Color).HasConversion<string>();
                });
                
                e.OwnsOne(u => u.Address, a =>
                {
                    a.Property(p => p.Street);
                    a.Property(p => p.Number);
                    a.Property(p => p.Floor);
                    a.Property(p => p.City);
                    a.Property(p => p.ZipCode);
                });
            });
            
            modelBuilder.Entity<Student>(e =>
            {
                e.Property(s => s.UserId)
                 .HasConversion(v => v.Value, v => new UserId(v))
                 .IsRequired();

                e.HasOne(s => s.User)
                 .WithMany()
                 .HasForeignKey(s => s.UserId)
                 .HasPrincipalKey(u => u.Id)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Master>(e =>
            {
                e.HasKey(m => m.Id);

                e.Property(m => m.UserId)
                 .HasConversion(v => v.Value, v => new UserId(v))
                 .IsRequired();

                e.HasOne(m => m.User)
                 .WithMany()
                 .HasForeignKey(m => m.UserId)
                 .HasPrincipalKey(u => u.Id)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // --- ApplicationUser: ponte para dom�nio ---
            modelBuilder.Entity<ApplicationUser>(e =>
            {
                e.Property(u => u.DomainUserId);
                e.HasIndex(u => u.DomainUserId);
            });

            modelBuilder.Entity<Exercise>().OwnsOne(e => e.Grade);
            modelBuilder.Entity<Exercise>().OwnsOne(e => e.BeltLevelMin);
            modelBuilder.Entity<Exercise>().OwnsOne(e => e.BeltLevelMax);


        }
    }
}