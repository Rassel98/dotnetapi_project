using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PkemonReviewApp.Dto;
using PkemonReviewApp.Interfaces;
using PkemonReviewApp.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PkemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository,IMapper mapper)
        {
          _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllReviewers()
        {
            var reviewers=_mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if(!ModelState.IsValid) 
                return BadRequest(); 
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId)) 
                return NotFound();
            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));
            if (!ModelState.IsValid) 
                return BadRequest();
            
            return Json(new {message="Reviewer data get successfully",data=reviewer});
        }
        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);
            if (!ModelState.IsValid) 
                return BadRequest();
            return Ok(reviews);

        }
    }
}
