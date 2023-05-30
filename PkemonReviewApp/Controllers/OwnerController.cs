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
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerReposotory ownerReposotory,ICountryRepository countryRepository,IMapper mapper)
        {
            _ownerReposotory = ownerReposotory;
           _countryRepository = countryRepository;
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
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateNewOwner([FromQuery]int countryId,[FromBody]OwnerDto ownerCreate)
        {
            if (ownerCreate == null) return BadRequest(ModelState);
            var owner=_ownerReposotory.GetOwners().Where(o=>o.LastName.ToLower()==ownerCreate.LastName.ToLower()).FirstOrDefault();   
            if(owner != null) 
            {
                ModelState.AddModelError("", "Owner allready exists");
                return StatusCode(422, ModelState);

            }
            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _countryRepository.GetCountry(countryId);
            if (!_ownerReposotory.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went to wrong for creating new owner");
                return StatusCode(500, ModelState);
            }
            return Json(new { message = "New Owner created Successfully", status = "Success" });
        }
        [HttpPut("{ownerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if(updatedOwner==null)return BadRequest(ModelState);
            if(ownerId!=updatedOwner.Id)
                return BadRequest(ModelState);
            if(!_ownerReposotory.OwnerExists(ownerId))
                return NotFound(ModelState);

            var ownerMap=_mapper.Map<Owner>(updatedOwner);
            if (!_ownerReposotory.UpdateOwner(ownerMap))
                return StatusCode(500, "internal server error");
            return StatusCode(200, new { message = "Successfully owner updated", status = "success" });
        }
        [HttpDelete("ownerId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerReposotory.OwnerExists(ownerId))
                return NotFound();
            var owner=_ownerReposotory.GetOwner(ownerId);
            if (!_ownerReposotory.DeleteOwner(owner))
                return StatusCode(500, "Internal server error");
            return StatusCode(200, new { message = "Owner deleted successfully" });
        }




    }
}
