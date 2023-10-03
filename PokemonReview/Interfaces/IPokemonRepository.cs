using PokemonReview.DTOs;
using PokemonReview.Models;

namespace PokemonReview.Interfaces;
public interface IPokemonRepository
{
    Task<ICollection<Pokemon>> GetPokemonsAsync();
    Task<Pokemon> GetPokemonTrimToUpper(PokemonDto pokemonDto);
    Task<Pokemon> GetPokemonByIdAsync(int id);
    Task<Pokemon> GetPokemonByNameAsync(string name);
    decimal GetPokemonByRatingAsync(int pokeId);
    bool PokemonExist(int pokeId);

    Task<bool> AddPokemon(int ownerId, int categoryId, Pokemon pokemon);
    Task<bool> UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    Task<bool> DeletePokemon(Pokemon pokemon);
    Task<bool> SaveChanges();

}
