using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PkemonReviewApp.Dto;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;
using System.Diagnostics.Metrics;

namespace PkemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerReposotory _ownerReposotory;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerReposotory ownerReposotory,IMapper mapper)
        {
            _ownerReposotory = ownerReposotory;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200,Type =typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int id)
        {
            if(!_ownerReposotory.OwnerExists(id))return  NotFound();
            var owner=_ownerReposotory.GetOwner(id);
            return Ok(owner);
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerReposotory.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Json(new {data= owners });
        }
        [HttpGet("/pokemon/{ownerid}")]
        [ProducesResponseType(200,Type =typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerid)
        {
            if(!_ownerReposotory.OwnerExists(ownerid))return NotFound();
            var owner=_mapper.Map<List<PokemonDto>>(_ownerReposotory.GetPokemonByOwner(ownerid));
            if(!ModelState.IsValid) { return BadRequest(); }
            return Json(new {data= owner});

        }




    }
}
