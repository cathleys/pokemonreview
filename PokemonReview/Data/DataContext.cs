using Microsoft.EntityFrameworkCore;
using PokemonReview.Models;

namespace PokemonReview.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<PokemonCategory> PokemonCategories { get; set; }
    public DbSet<PokemonOwner> PokemonOwners { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<PokemonCategory>()
        .HasKey(pc => new { pc.PokemonId, pc.CategoryId });

        modelBuilder.Entity<PokemonCategory>()
        .HasOne(pc => pc.Pokemon)
        .WithMany(p => p.PokemonCategories)
        .HasForeignKey(pc => pc.PokemonId);
        modelBuilder.Entity<PokemonCategory>()
        .HasOne(pc => pc.Category)
        .WithMany(p => p.PokemonCategories)
        .HasForeignKey(pc => pc.CategoryId);




        modelBuilder.Entity<PokemonOwner>()
               .HasKey(po => new { po.PokemonId, po.OwnerId });

        modelBuilder.Entity<PokemonOwner>()
        .HasOne(po => po.Pokemon)
        .WithMany(p => p.PokemonOwners)
        .HasForeignKey(po => po.PokemonId);
        modelBuilder.Entity<PokemonOwner>()
        .HasOne(po => po.Owner)
        .WithMany(p => p.PokemonOwners)
        .HasForeignKey(po => po.OwnerId);
    }
}
