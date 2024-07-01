﻿// <auto-generated />
using System;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymManagement.Infrastructure.Common.Persistence.Migrations
{
    [DbContext(typeof(GymManagementDbContext))]
    [Migration("20240629032840_AddedGymAndAdmins")]
    partial class AddedGymAndAdmins
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GymManagement.Domain.Admins.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2150e333-8fdc-42a3-9474-1a3956d46de8")
                        });
                });

            modelBuilder.Entity("GymManagement.Domain.Gyms.Gym", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("_maxRooms")
                        .HasColumnType("int")
                        .HasColumnName("MaxRooms");

                    b.Property<string>("_roomIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("RoomIds");

                    b.Property<string>("_trainerIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TrainerIds");

                    b.HasKey("Id");

                    b.ToTable("Gyms");
                });

            modelBuilder.Entity("GymManagement.Domain.Subscriptions.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SubscriptionType")
                        .HasColumnType("int");

                    b.Property<string>("_gymIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("GymIds");

                    b.Property<int>("_maxGyms")
                        .HasColumnType("int")
                        .HasColumnName("MaxGyms");

                    b.HasKey("Id");

                    b.ToTable("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
