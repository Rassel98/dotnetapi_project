using AutoMapper;
using PkemonReviewApp.Data;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;

namespace PkemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
      
        public CountryRepository(DataContext context)
        {
           _context = context;
          
        }

        public Country Get(int id)
        {
            return _context.Countrys.Where(c=>c.Id==id).FirstOrDefault();
        }

        public ICollection<Country> GetAll()
        {
            return _context.Countrys.ToList();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
           
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return _context.Owners.Where(c => c.Country.Id == countryId).ToList();
        }

        public bool HasCountry(int id)
        {
            return _context.Countrys.Any(c=>c.Id==id);
        }
    }
}
