using PkemonReviewApp.Models;

namespace PkemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        Category GetCategory(string name);
        bool CategoryExists(int id);
        bool CreateCategory(Category category);
        bool Save();

    }
}
