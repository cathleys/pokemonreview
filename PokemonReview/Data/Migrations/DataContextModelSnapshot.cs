﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokemonReview.Data;

#nullable disable

namespace PokemonReview.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("PokemonReview.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PokemonReview.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("PokemonReview.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CountryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gym")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("PokemonReview.Models.Pokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pokemons");
                });

            modelBuilder.Entity("PokemonReview.Models.PokemonCategory", b =>
                {
                    b.Property<int>("PokemonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PokemonId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PokemonCategories");
                });

            modelBuilder.Entity("PokemonReview.Models.PokemonOwner", b =>
                {
                    b.Property<int>("PokemonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PokemonId", "OwnerId");

                    b.HasIndex("OwnerId");

                    b.ToTable("PokemonOwners");
                });

            modelBuilder.Entity("PokemonReview.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PokemonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReviewerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PokemonId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("PokemonReview.Models.Reviewer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Reviewers");
                });

            modelBuilder.Entity("PokemonReview.Models.Owner", b =>
                {
                    b.HasOne("PokemonReview.Models.Country", "Country")
                        .WithMany("Owners")
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("PokemonReview.Models.PokemonCategory", b =>
                {
                    b.HasOne("PokemonReview.Models.Category", "Category")
                        .WithMany("PokemonCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokemonReview.Models.Pokemon", "Pokemon")
                        .WithMany("PokemonCategories")
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Pokemon");
                });

            modelBuilder.Entity("PokemonReview.Models.PokemonOwner", b =>
                {
                    b.HasOne("PokemonReview.Models.Owner", "Owner")
                        .WithMany("PokemonOwners")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokemonReview.Models.Pokemon", "Pokemon")
                        .WithMany("PokemonOwners")
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Pokemon");
                });

            modelBuilder.Entity("PokemonReview.Models.Review", b =>
                {
                    b.HasOne("PokemonReview.Models.Pokemon", "Pokemon")
                        .WithMany("Reviews")
                        .HasForeignKey("PokemonId");

                    b.HasOne("PokemonReview.Models.Reviewer", "Reviewer")
                        .WithMany("Reviews")
                        .HasForeignKey("ReviewerId");

                    b.Navigation("Pokemon");

                    b.Navigation("Reviewer");
                });

            modelBuilder.Entity("PokemonReview.Models.Category", b =>
                {
                    b.Navigation("PokemonCategories");
                });

            modelBuilder.Entity("PokemonReview.Models.Country", b =>
                {
                    b.Navigation("Owners");
                });

            modelBuilder.Entity("PokemonReview.Models.Owner", b =>
                {
                    b.Navigation("PokemonOwners");
                });

            modelBuilder.Entity("PokemonReview.Models.Pokemon", b =>
                {
                    b.Navigation("PokemonCategories");

                    b.Navigation("PokemonOwners");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("PokemonReview.Models.Reviewer", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
