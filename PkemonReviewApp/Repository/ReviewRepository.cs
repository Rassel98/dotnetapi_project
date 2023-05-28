using PkemonReviewApp.Data;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;

namespace PkemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public ICollection<Review> GetAllReviewsOfAPokemon(int pokeid)
        {
           return _context.Reviews.Where(r=>r.Pokemon.Id == pokeid).ToList();
        }

        public Review GetReview(int id)
        {
           return _context.Reviews.Where(review => review.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public bool HasReview(int id)
        {
           return _context.Reviews.Any(re=>re.Id== id);
        }

        public bool Save()
        {
        var saved=   _context.SaveChanges();
            return saved>0?true:false;
        }
    }
}
