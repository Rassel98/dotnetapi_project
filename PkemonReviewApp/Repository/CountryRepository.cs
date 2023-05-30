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

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return save();
        }

        public Country GetCountry(int id)
        {
            return _context.Countrys.Where(c=>c.Id==id).FirstOrDefault();
        }

        public ICollection<Country> GetAllCountrys()
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

        public bool save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return save();
        }
    }
}
