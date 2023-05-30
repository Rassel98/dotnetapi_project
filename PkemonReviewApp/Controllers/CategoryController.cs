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
            try
            {
                var category = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Json(new { message = "Data comes successfully", data = category });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }


        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id)
        {
            try
            {
                if (!_categoryRepository.CategoryExists(id))
                {
                    return NotFound();
                }
                var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(id));
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                //return Ok(category);
                return Json(new { message = "Data comes successfully", data = category });
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategory(int categoryId)
        {
            try
            {
                var pokemon = _mapper.Map<List<Pokemon>>(_categoryRepository.GetPokemonByCategory(categoryId));

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                //return Ok(category);
                return Json(new { message = "Data comes successfully", data = pokemon });
            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto category)
        {
            try
            {
                if (category == null) return BadRequest(ModelState);
                var cheakCategory = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToLower() == category.Name.TrimEnd().ToLower()).FirstOrDefault();
                if (cheakCategory != null)
                {
                    ModelState.AddModelError("", "Category Already Exists");
                    return StatusCode(422, ModelState);
                }
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var categoryMap = _mapper.Map<Category>(category);
                if (!_categoryRepository.CreateCategory(categoryMap))
                {
                    ModelState.AddModelError("", "Somthing went to wrong for creating new category");
                    return StatusCode(500, ModelState);
                }
                return Json(new { Message = "Category created successfully" });
            }catch  (Exception e)
            {
                return BadRequest(e.Message);
            }


        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updateCategory)
        {
            try
            {
                if (updateCategory == null) return BadRequest();

                if (categoryId != updateCategory.Id) return BadRequest(ModelState);

                if (!_categoryRepository.CategoryExists(categoryId)) return NotFound();
                var categoryMap = _mapper.Map<Category>(updateCategory);
                if (!_categoryRepository.UpdateCategory(categoryMap))
                {
                    return StatusCode(500, "Internal server error");
                }
                return StatusCode(201, new { Message = "category updated successfull", status = "success" });
            }catch(Exception e) {
                return BadRequest();
            }

        }
        [HttpDelete("categoryId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCategory(int categoryId)
        {
            try
            {
                if (!_categoryRepository.CategoryExists(categoryId))
                    return NotFound();
                var category = _categoryRepository.GetCategory(categoryId);
                if (!_categoryRepository.DeleteCategory(category))
                    return StatusCode(500, "Internal server error");
                return StatusCode(200, new { Message = "Category deleted successfully" });
            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
       

    }
}
