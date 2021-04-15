﻿// <auto-generated />
using System;
using AirClub.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AirClub.Migrations
{
    [DbContext(typeof(AirClubDbContext))]
    [Migration("20200508102457_Tours")]
    partial class Tours
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AirClub.Model.Db.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgeBefore")
                        .HasColumnType("int");

                    b.Property<int>("AgeFrom")
                        .HasColumnType("int");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhysReqs")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Services");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Service");
                });

            modelBuilder.Entity("AirClub.Model.Db.Tour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tours");
                });

            modelBuilder.Entity("Human", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Humen");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Human");
                });

            modelBuilder.Entity("Special", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Specials");
                });

            modelBuilder.Entity("AirClub.Model.Db.ServiceActiveRest", b =>
                {
                    b.HasBaseType("AirClub.Model.Db.Service");

                    b.Property<int>("MaxCountPeople")
                        .HasColumnType("int");

                    b.Property<int>("MinCountPeople")
                        .HasColumnType("int");

                    b.Property<int>("TourId")
                        .HasColumnType("int");

                    b.HasIndex("TourId");

                    b.HasDiscriminator().HasValue("ServiceActiveRest");
                });

            modelBuilder.Entity("AirClub.Model.Db.ServiceCompetition", b =>
                {
                    b.HasBaseType("AirClub.Model.Db.Service");

                    b.Property<DateTime>("DateBegin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("ServiceCompetition");
                });

            modelBuilder.Entity("AirClub.Model.Db.ServiceCourse", b =>
                {
                    b.HasBaseType("AirClub.Model.Db.Service");

                    b.Property<int>("CourseDuration")
                        .HasColumnType("int");

                    b.Property<int>("ExersiceDuration")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("ServiceCourse");
                });

            modelBuilder.Entity("AirClub.Model.Db.Client", b =>
                {
                    b.HasBaseType("Human");

                    b.Property<string>("PasportData")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Client");
                });

            modelBuilder.Entity("Employee", b =>
                {
                    b.HasBaseType("Human");

                    b.Property<string>("AccessCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EdDocGetDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EducationDoc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SpecialId")
                        .HasColumnType("int");

                    b.HasIndex("SpecialId");

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("AirClub.Model.Db.ServiceActiveRest", b =>
                {
                    b.HasOne("AirClub.Model.Db.Tour", "Tour")
                        .WithMany()
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Employee", b =>
                {
                    b.HasOne("Special", "Special")
                        .WithMany("Employees")
                        .HasForeignKey("SpecialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
