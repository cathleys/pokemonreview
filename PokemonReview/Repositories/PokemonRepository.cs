using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repositories;
public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;
    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPokemon(int ownerId,
    int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = await _context.Owners
        .Where(o => o.Id == ownerId)
        .FirstOrDefaultAsync();

        var pokemonCategoryEntity = await _context.Categories
        .Where(c => c.Id == categoryId)
        .FirstOrDefaultAsync();

        //Creating object to insert owner and pokemon
        var pokemonOwner = new PokemonOwner()
        {
            Owner = pokemonOwnerEntity,
            Pokemon = pokemon

        };

        //Going to track and add the object to joint table pokemonOwners
        _context.Add(pokemonOwner);

        //Creating object to insert  category and pokemon
        var pokemonCategory = new PokemonCategory()
        {
            Category = pokemonCategoryEntity,
            Pokemon = pokemon,
        };

        //Going to track and add the object to joint table pokemonCategory
        _context.Add(pokemonCategory);
        _context.Add(pokemon);

        //Will then SQL commands be generated 
        // and sends these entities to db.
        return await SaveChanges();

    }

    public async Task<bool> DeletePokemon(Pokemon pokemon)
    {
        _context.Remove(pokemon);
        return await SaveChanges();
    }

    public async Task<Pokemon> GetPokemonByIdAsync(int id)
    {
        return await _context.Pokemons.FindAsync(id);
    }

    public async Task<Pokemon> GetPokemonByNameAsync(string name)
    {
        return await _context.Pokemons.SingleOrDefaultAsync(p => p.Name == name);
    }

    public decimal GetPokemonByRatingAsync(int pokeId)
    {
        var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

        if (!review.Any()) return 0;

        return (decimal)review.Sum(r => r.Rating) / review.Count();
    }

    public async Task<IEnumerable<Pokemon>> GetPokemonsAsync()
    {
        return await _context.Pokemons.OrderBy(p => p.Id).ToListAsync();
    }

    public bool PokemonExist(int pokeId)
    {
        return _context.Pokemons.Any(p => p.Id == pokeId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        _context.Update(pokemon);
        return await SaveChanges();
    }


}
