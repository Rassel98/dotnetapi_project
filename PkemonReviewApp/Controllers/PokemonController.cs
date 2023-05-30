using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PkemonReviewApp.Dto;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;
using PkemonReviewApp.Repository;

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
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateNewPokemon([FromQuery] int ownerId, [FromQuery] int CategoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if(pokemonCreate ==null)
                return BadRequest(ModelState);

            var pokemons=_pokemonRepository.GetPokemons().Where(c=>c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(pokemons != null)
            {
                ModelState.AddModelError("", "Already Exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if(!_pokemonRepository.CreatePokemon(ownerId, CategoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went to wrong");
                return StatusCode(StatusCodes.Status500InternalServerError,ModelState);

            }
       
           return new CreatedAtRouteResult(nameof(ModelState), new { message = "Data saved successfully", StatusCodes.Status201Created });
            //return Json(new {message="Data saved successfully",StatusCodes.Status201Created});


        }
        [HttpPut("{pokeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdatedPokemon(int pokeId,[FromBody] PokemonDto updatePokemon)
        {
            if (updatePokemon == null)
                return BadRequest(ModelState);

            if (updatePokemon.Id != pokeId)
            {
               
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(updatePokemon);

            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went to wrong");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);

            }

            return new CreatedAtRouteResult(nameof(ModelState), new { message = "Data saved successfully", StatusCodes.Status200OK });
            //return Json(new {message="Data saved successfully",StatusCodes.Status201Created});


        }
        [HttpDelete("pokeId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteOwner(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pokemon = _pokemonRepository.GetPokemon(pokeId);
            if (!_pokemonRepository.DeletePokemon(pokemon))
                return StatusCode(500, "Internal server error");
            return StatusCode(200, new { message = "Pokemon deleted successfully" });
        }

    }
}
