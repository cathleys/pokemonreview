using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Models;

namespace PokemonReview.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }


    //In Create Method, When Add is called, EF/DbContext will start tracking the object
    //for adding, updating, deleting
    public async Task<bool> AddCategory(Category category)
    {
        _context.Add(category);
        return await SaveChanges();
    }

    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    public async Task<bool> DeleteCategory(Category category)
    {
        _context.Remove(category);
        return await SaveChanges();
    }

    public async Task<ICollection<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategory(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId)
    {
        return await _context.PokemonCategories
        .Where(c => c.CategoryId == categoryId)
        .Select(p => p.Pokemon).ToListAsync();
    }


    //SaveChanges - this is the method when the actual sql is
    //going to be generated and the object, through EF and send to database.
    public async Task<bool> SaveChanges()
    {
        var saved = await _context.SaveChangesAsync() > 0;

        return saved;

    }

    public async Task<bool> UpdateCategory(Category category)
    {
        _context.Update(category);
        return await SaveChanges();
    }
}
