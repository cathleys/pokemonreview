using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repositories;
public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddOwner(Owner owner)
    {
        _context.Add(owner);
        return await SaveChanges();
    }

    public async Task<bool> DeleteOwner(Owner owner)
    {
        _context.Remove(owner);
        return await SaveChanges();
    }

    public async Task<Owner> GetOwner(int id)
    {
        return await _context.Owners
        .Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ICollection<Owner>> GetOwners()
    {
        return await _context.Owners.ToListAsync();
    }

    public async Task<ICollection<Owner>> GetOwnersByPokemon(int pokeId)
    {
        return await _context.PokemonOwners.Where(p => p.Pokemon.Id == pokeId)
        .Select(o => o.Owner).ToListAsync();
    }

    public async Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId)
    {
        return await _context.PokemonOwners.Where(o => o.Owner.Id == ownerId)
        .Select(p => p.Pokemon).ToListAsync();
    }

    public bool OwnerExists(int ownerId)
    {
        return _context.Owners.Any(o => o.Id == ownerId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateOwner(Owner owner)
    {
        _context.Update(owner);
        return await SaveChanges();
    }
}
