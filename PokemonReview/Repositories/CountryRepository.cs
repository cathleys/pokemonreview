using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repositories;
public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;

    public CountryRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddCountry(Country country)
    {
        _context.Add(country);
        return await SaveChanges();
    }

    public bool CountryExists(int id)
    {
        return _context.Countries.Any(c => c.Id == id);
    }

    public async Task<bool> DeleteCountry(Country country)
    {
        _context.Remove(country);
        return await SaveChanges();
    }

    public async Task<ICollection<Country>> GetCountries()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country> GetCountry(int id)
    {
        return await _context.Countries
        .Where(c => c.Id == id).FirstOrDefaultAsync(); //could also be ...Countries.FindAsync(id);
    }

    public async Task<Country> GetCountryByOwner(int ownerId)
    {
        return await _context.Owners
        .Where(o => o.Id == ownerId)
        .Select(c => c.Country)
        .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Owner>> GetOwnersByCountry(int countryId)
    {
        return await _context.Owners
        .Where(c => c.Country.Id == countryId)
        .ToListAsync();
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateCountry(Country country)
    {
        _context.Update(country);
        return await SaveChanges();
    }
}
