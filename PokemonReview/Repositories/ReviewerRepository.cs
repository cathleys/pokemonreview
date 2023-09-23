using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repositories;
public class ReviewerRepository : IReviewerRepository
{
    private readonly DataContext _context;

    public ReviewerRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddReviewer(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return await SaveChanges();
    }

    public async Task<bool> DeleteReviewer(Reviewer reviewer)
    {
        _context.Remove(reviewer);
        return await SaveChanges();
    }

    public async Task<Reviewer> GetReviewer(int reviewerId)
    {
        return await _context.Reviewers.Where(r => r.Id == reviewerId)
        .Include(r => r.Reviews)
        .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Reviewer>> GetReviewers()
    {
        return await _context.Reviewers
        .Include(r => r.Reviews)
        .ToListAsync();
    }

    public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
    {
        return await _context.Reviews.Where(r => r.Reviewer.Id == reviewerId)
        .ToListAsync();
    }

    public bool ReviewerExists(int reviewerId)
    {
        return _context.Reviewers.Any(r => r.Id == reviewerId);
    }

    public async Task<bool> SaveChanges()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateReviewer(Reviewer reviewer)
    {
        _context.Update(reviewer);
        return await SaveChanges();
    }
}
