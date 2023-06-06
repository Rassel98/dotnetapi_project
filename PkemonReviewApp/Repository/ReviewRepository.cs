using PkemonReviewApp.Data;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;
using Microsoft.EntityFrameworkCore;

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

        public bool DeleteReview(Review review)
        {
        
            _context.Remove(review);
            return Save();
        }

        public ICollection<Review> GetAllReviewsOfAPokemon(int pokeid)
        {
           return _context.Reviews.Where(r=>r.Pokemon.Id == pokeid).ToList();
        }

        public Review GetReview(int id)
        {
           return _context.Reviews.Include(c=>c.Pokemon).Include(r=>r.Reviewer).Where(review => review.Id == id).FirstOrDefault();
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

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }
    }
}
