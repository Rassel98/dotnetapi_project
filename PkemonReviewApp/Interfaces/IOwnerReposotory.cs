using PkemonReviewApp.Models;

namespace PkemonReviewApp.Interfaces
{
    public interface IOwnerReposotory
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnerOfPokemon(int pokeid);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool OwnerExists(int ownerId);
        bool CreateOwner(Owner owner);
        bool Save();

    }
}
