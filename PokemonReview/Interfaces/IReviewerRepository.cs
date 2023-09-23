using PokemonReview.Models;

namespace PokemonReview.Interfaces;
public interface IReviewerRepository
{
    Task<ICollection<Reviewer>> GetReviewers();
    Task<Reviewer> GetReviewer(int reviewerId);
    Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId);
    bool ReviewerExists(int reviewerId);

    Task<bool> AddReviewer(Reviewer reviewer);
    Task<bool> UpdateReviewer(Reviewer reviewer);
    Task<bool> DeleteReviewer(Reviewer reviewer);

    Task<bool> SaveChanges();
}
