using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repositories;
public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddReview(Review review)
    {
        _context.Add(review);
        return await SaveChanges();

    }

    public async Task<bool> DeleteReview(Review review)
    {
        _context.Remove(review);
        return await SaveChanges();
    }

    public async Task<bool> DeleteReviews(List<Review> reviews)
    {
        _context.RemoveRange(reviews);
        return await SaveChanges();
    }

    public async Task<Review> GetReview(int reviewId)
    {
        return await _context.Reviews.Where(r => r.Id == reviewId)
        .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Review>> GetReviews()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<ICollection<Review>> GetReviewsByPokemon(int pokeId)
    {
        return await _context.Reviews.Where(p => p.Pokemon.Id == pokeId)
        .ToListAsync();

    }

    public bool ReviewExists(int reviewId)
    {
        return _context.Reviews.Any(r => r.Id == reviewId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateReview(Review review)
    {
        _context.Update(review);
        return await SaveChanges();
    }
}