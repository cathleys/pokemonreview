using PokemonReview.Models;

namespace PokemonReview.Interfaces;
public interface IOwnerRepository
{
    Task<ICollection<Owner>> GetOwners();

    Task<Owner> GetOwner(int id);

    Task<ICollection<Owner>> GetOwnersByPokemon(int pokeId);
    Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId);

    bool OwnerExists(int ownerId);

    //Create,Update,Delete Method toss the entire entity to db
    Task<bool> AddOwner(Owner owner);
    Task<bool> UpdateOwner(Owner owner);
    Task<bool> DeleteOwner(Owner owner);
    Task<bool> SaveChanges();
}
