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
            var country = _mapper.Map<List<Country>>(_countryRepository.GetAllCountrys());
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
            var country=_countryRepository.GetCountry(id);
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
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateNewCountry([FromBody]CountryDto country)
        {
            if(country==null)return BadRequest();

            var isCountry = _countryRepository.GetAllCountrys().Where(c => c.Name.Trim().ToLower() == country.Name.TrimEnd().ToLower()).FirstOrDefault();
            if (isCountry != null)
            {
                ModelState.AddModelError("", "country allready exists");
                return StatusCode(422, ModelState);
            }
            var countryMap=_mapper.Map<Country>(country);
            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Somthing went to wrong for creating new category");
                return StatusCode(500, ModelState);
            }
           
            return Json(new { Message = "new country created successfully" });


        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCountry(int countryId, [FromBody]CountryDto newCountry)
        {
            if(newCountry==null)
                return BadRequest();
            if(countryId!=newCountry.Id)
                return BadRequest();
            if(!_countryRepository.HasCountry(countryId))
                return NotFound();
            var countryMap = _mapper.Map<Country>(newCountry);
            if (!_countryRepository.UpdateCountry(countryMap))
            {
                return StatusCode(500, "internal server error");
            }
            return StatusCode(201, new { Message = "country updated successfully", status = "success" });
        }
    }
}
