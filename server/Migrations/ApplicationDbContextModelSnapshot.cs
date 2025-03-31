﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using server.Data;

#nullable disable

namespace server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("server.Models.Master", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Belt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Masters");
                });

            modelBuilder.Entity("server.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Belt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EnrollDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Height")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Weight")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("server.Models.Workout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Workouts");
                });

            modelBuilder.Entity("server.Models.WorkoutMaster", b =>
                {
                    b.Property<int>("WorkoutId")
                        .HasColumnType("int");

                    b.Property<int>("MasterId")
                        .HasColumnType("int");

                    b.HasKey("WorkoutId", "MasterId");

                    b.HasIndex("MasterId");

                    b.ToTable("WorkoutMasters");
                });

            modelBuilder.Entity("server.Models.WorkoutStudent", b =>
                {
                    b.Property<int>("WorkoutId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("WorkoutId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("WorkoutStudents");
                });

            modelBuilder.Entity("server.Models.WorkoutMaster", b =>
                {
                    b.HasOne("server.Models.Master", "Master")
                        .WithMany("WorkoutMasters")
                        .HasForeignKey("MasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.Workout", "Workout")
                        .WithMany("WorkoutMasters")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Master");

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("server.Models.WorkoutStudent", b =>
                {
                    b.HasOne("server.Models.Student", "Student")
                        .WithMany("WorkoutStudents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("server.Models.Workout", "Workout")
                        .WithMany("WorkoutStudents")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("server.Models.Master", b =>
                {
                    b.Navigation("WorkoutMasters");
                });

            modelBuilder.Entity("server.Models.Student", b =>
                {
                    b.Navigation("WorkoutStudents");
                });

            modelBuilder.Entity("server.Models.Workout", b =>
                {
                    b.Navigation("WorkoutMasters");

                    b.Navigation("WorkoutStudents");
                });
#pragma warning restore 612, 618
        }
    }
}
