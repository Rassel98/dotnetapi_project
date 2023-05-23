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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository,IMapper mapper)
        {
         _countryRepository = countryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllCountry()
        {
            var country = _mapper.Map<List<Country>>(_countryRepository.GetAll());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Json(new { message = "Data comes successfully", data = country });

        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int id)
        {
            if(!_countryRepository.HasCountry(id))
                return NotFound();
            var country=_countryRepository.Get(id);
            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryofOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid) return BadRequest();
            return Json(new { message = "Data get successfully", data = country });
        }
    }
}
