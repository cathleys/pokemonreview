using PokemonReview.Models;

namespace PokemonReview.Interfaces;
public interface IReviewRepository
{
    Task<ICollection<Review>> GetReviews();
    Task<Review> GetReview(int reviewId);

    Task<ICollection<Review>> GetReviewsByPokemon(int pokeId);
    bool ReviewExists(int reviewId);

    Task<bool> AddReview(Review review);

    Task<bool> UpdateReview(Review review);
    Task<bool> DeleteReview(Review review);
    Task<bool> DeleteReviews(List<Review> reviews);
    Task<bool> SaveChanges();

}
