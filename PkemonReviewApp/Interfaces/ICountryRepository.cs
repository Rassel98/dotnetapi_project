using PkemonReviewApp.Models;

namespace PkemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetAll();
        Country Get(int id);
        bool HasCountry(int id);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner>GetOwnersFromACountry(int countryId);
    }
}
