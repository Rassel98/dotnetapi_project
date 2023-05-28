using PkemonReviewApp.Models;

namespace PkemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int id);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool ReviewerExists(int reviewerid);
        bool CreateReviewer(Reviewer reviewer);
        bool Save();
    }
}
