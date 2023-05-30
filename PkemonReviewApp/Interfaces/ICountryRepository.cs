using PkemonReviewApp.Models;

namespace PkemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetAllCountrys();
        Country GetCountry(int id);
        bool HasCountry(int id);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner>GetOwnersFromACountry(int countryId);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool save();
    }
}
