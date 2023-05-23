using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PkemonReviewApp.Dto;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;

namespace PkemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository ,IMapper mapper)
        {
           _pokemonRepository = pokemonRepository;
          _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType( 200,Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons=_mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
           
        }
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokemon = _pokemonRepository.GetPokemon(pokeId);
            var pk = new Pokemon
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                BirthDate = pokemon.BirthDate
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pk);

        }
        [HttpGet("/rating/{pokeId}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Json(new { rating });

        }

    }
}
