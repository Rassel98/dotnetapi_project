using Microsoft.EntityFrameworkCore;
using PkemonReviewApp.Data;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;

namespace PkemonReviewApp.Repository
{
    public class ReviewerRepository :IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
           _context = context;
        }

        public Reviewer GetReviewer(int id)
        {
            return _context.Reviewers.Where(re => re.Id == id).Include(e=>e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
          return  _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
           return _context.Reviews.Where(re=>re.Reviewer.Id==reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerid)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerid);
        }
    }
}
