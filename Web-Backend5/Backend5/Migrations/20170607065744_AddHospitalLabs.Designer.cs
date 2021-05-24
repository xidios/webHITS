using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Backend5.Data;

namespace Backend5.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170607065744_AddHospitalLabs")]
    partial class AddHospitalLabs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend5.Models.Hospital", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("Backend5.Models.HospitalLab", b =>
                {
                    b.Property<int>("HospitalId");

                    b.Property<int>("LabId");

                    b.HasKey("HospitalId", "LabId");

                    b.HasIndex("LabId");

                    b.ToTable("HospitalLabs");
                });

            modelBuilder.Entity("Backend5.Models.HospitalPhone", b =>
                {
                    b.Property<int>("HospitalId");

                    b.Property<int>("PhoneId");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("HospitalId", "PhoneId");

                    b.ToTable("HospitalPhones");
                });

            modelBuilder.Entity("Backend5.Models.Lab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Labs");
                });

            modelBuilder.Entity("Backend5.Models.LabPhone", b =>
                {
                    b.Property<int>("LabId");

                    b.Property<int>("PhoneId");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("LabId", "PhoneId");

                    b.ToTable("LabPhones");
                });

            modelBuilder.Entity("Backend5.Models.Ward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HospitalId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("HospitalId");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("Backend5.Models.HospitalLab", b =>
                {
                    b.HasOne("Backend5.Models.Hospital", "Hospital")
                        .WithMany("Labs")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Backend5.Models.Lab", "Lab")
                        .WithMany("Hospitals")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Backend5.Models.HospitalPhone", b =>
                {
                    b.HasOne("Backend5.Models.Hospital", "Hospital")
                        .WithMany("Phones")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Backend5.Models.LabPhone", b =>
                {
                    b.HasOne("Backend5.Models.Lab", "Lab")
                        .WithMany("Phones")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Backend5.Models.Ward", b =>
                {
                    b.HasOne("Backend5.Models.Hospital", "Hospital")
                        .WithMany("Wards")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
