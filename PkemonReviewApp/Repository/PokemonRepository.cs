using PkemonReviewApp.Data;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;

namespace PkemonReviewApp.Repository
{
    public class PokemonRepository: IPokemonRepository
    {
      private  readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
            
        }

  

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity=_context.Owners.Where(o=>o.Id==ownerId).FirstOrDefault();
            var categoryPokemonEntity=_context.Categories.Where(c=>c.Id==categoryId).FirstOrDefault();
            var pokemonOwner = new PokemonOwner()
            {
                Pokemon=pokemon,
                Owner=pokemonOwnerEntity
            };
            _context.Add(pokemonOwner);
            var pokemonCategory = new PokemonCategory()
            {
                Category = categoryPokemonEntity,
                Pokemon = pokemon
            };
            _context.Add(pokemonCategory);
            _context.Add(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
           var reviews=_context.Reviews.Where(p => p.Id == pokeId);
            if (reviews.Count() <= 0){
                return 0;
            }
            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public ICollection<Pokemon>GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save> 0 ? true : false;
        }
    }
}
