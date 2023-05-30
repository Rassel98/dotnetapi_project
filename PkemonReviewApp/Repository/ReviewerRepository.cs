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

        public bool CreateReviewer(Reviewer reviewer)
        {
           _context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
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

        public bool Save()
        {
           var saved= _context.SaveChanges();
            return saved>0?true:false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
