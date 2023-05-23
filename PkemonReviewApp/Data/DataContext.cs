﻿using Microsoft.EntityFrameworkCore;
using PkemonReviewApp.Models;

namespace PkemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options):base(options) 
        {
        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new {pc.PokemonId,pc.CategoryId});

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(c => c.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.CategoryId);



            modelBuilder.Entity<PokemonOwner>()
                .HasKey(po => new {po.PokemonId,po.OwnerId});
            modelBuilder.Entity<PokemonOwner>()
               .HasOne(p => p.Pokemon)
               .WithMany(pc => pc.PokemonOwners)
               .HasForeignKey(p=> p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(o => o.Owner)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(o => o.OwnerId);

        }
    }
   
}
