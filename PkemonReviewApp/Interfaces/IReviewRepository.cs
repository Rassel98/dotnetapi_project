using PkemonReviewApp.Models;

namespace PkemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetAllReviewsOfAPokemon(int pokeid);
        bool HasReview(int id);

    }
}
