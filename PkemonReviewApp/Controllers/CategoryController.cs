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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetAllCategory()
        {
            var category = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Json(new {message="Data comes successfully",data=category});
         
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound();
            }
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(id));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            //return Ok(category);
             return Json(new {message="Data comes successfully",data = category });
        }


        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategory(int categoryId)
        {
          
            var pokemon = _mapper.Map<List<Pokemon>>(_categoryRepository.GetPokemonByCategory(categoryId));
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //return Ok(category);
            return Json(new { message = "Data comes successfully", data = pokemon });
        }
       

    }
}
