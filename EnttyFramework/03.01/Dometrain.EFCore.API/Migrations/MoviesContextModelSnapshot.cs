﻿// <auto-generated />
using System;
using Dometrain.EFCore.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    [DbContext(typeof(MoviesContext))]
    partial class MoviesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dometrain.EFCore.API.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Drama"
                        });
                });

            modelBuilder.Entity("Dometrain.EFCore.API.Models.Movie", b =>
                {
                    b.Property<int>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Identifier"));

                    b.Property<string>("AgeRating")
                        .IsRequired()
                        .HasColumnType("char(32)");

                    b.Property<decimal>("InternetRating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MainGenreId")
                        .HasColumnType("int");

                    b.Property<string>("ReleaseDate")
                        .IsRequired()
                        .HasColumnType("char(8)");

                    b.Property<string>("Synopsis")
                        .HasColumnType("varchar(max)")
                        .HasColumnName("Plot");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.HasKey("Identifier");

                    b.HasIndex("MainGenreId");

                    b.ToTable("Movies", (string)null);

                    b.HasData(
                        new
                        {
                            Identifier = 1,
                            AgeRating = "Adolescent",
                            InternetRating = 0m,
                            MainGenreId = 1,
                            ReleaseDate = "19990910",
                            Synopsis = "Ed Norton and Brad Pitt have a couple of fist fights with each other.",
                            Title = "Fight Club"
                        });
                });

            modelBuilder.Entity("Dometrain.EFCore.API.Models.Movie", b =>
                {
                    b.HasOne("Dometrain.EFCore.API.Models.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("MainGenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Dometrain.EFCore.API.Models.Person", "Actors", b1 =>
                        {
                            b1.Property<int>("MovieIdentifier")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("MovieIdentifier", "Id");

                            b1.ToTable("Movie_Actors", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("MovieIdentifier");

                            b1.HasData(
                                new
                                {
                                    MovieIdentifier = 1,
                                    Id = 1,
                                    FirstName = "Edward",
                                    LastName = "Norton"
                                },
                                new
                                {
                                    MovieIdentifier = 1,
                                    Id = 2,
                                    FirstName = "Brad",
                                    LastName = "Pitt"
                                });
                        });

                    b.OwnsOne("Dometrain.EFCore.API.Models.Person", "Director", b1 =>
                        {
                            b1.Property<int>("MovieIdentifier")
                                .HasColumnType("int");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("MovieIdentifier");

                            b1.ToTable("Movie_Directors", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("MovieIdentifier");

                            b1.HasData(
                                new
                                {
                                    MovieIdentifier = 1,
                                    FirstName = "David",
                                    LastName = "Fincher"
                                });
                        });

                    b.Navigation("Actors");

                    b.Navigation("Director")
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Dometrain.EFCore.API.Models.Genre", b =>
                {
                    b.Navigation("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}
