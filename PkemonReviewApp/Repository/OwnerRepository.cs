﻿using PkemonReviewApp.Data;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;

namespace PkemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerReposotory
    {
        private readonly DataContext context;

        public OwnerRepository(DataContext context)
        {
            this.context = context;
        }
        public Owner GetOwner(int id)
        {
            return context.Owners.Where(o => o.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfPokemon(int pokeid)
        {
            return context.PokemonOwners.Where(p=>p.Pokemon.Id== pokeid).Select(o=>o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            
            return context.PokemonOwners.Where(o=>o.Owner.Id==ownerId).Select(p=>p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return context.Owners.Any(o => o.Id == ownerId);
        }
    }
}
