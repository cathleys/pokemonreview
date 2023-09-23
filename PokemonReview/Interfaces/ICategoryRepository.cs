using PokemonReview.Models;

namespace PokemonReview.Repositories;
public interface ICategoryRepository
{
    Task<ICollection<Category>> GetCategories();

    Task<Category> GetCategory(int id);
    Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
    bool CategoryExists(int id);
    Task<bool> AddCategory(Category category);
    Task<bool> UpdateCategory(Category category);
    Task<bool> DeleteCategory(Category category);
    Task<bool> SaveChanges();
}
