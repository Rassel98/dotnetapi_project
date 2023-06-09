﻿using AutoMapper;
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
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody]ReviewerDto reviewerCreate)
        {
            if(reviewerCreate ==null)
                return BadRequest(ModelState);
            var reviewer=_reviewerRepository.GetReviewers().Where(r=>r.LastName.ToUpper()==reviewerCreate.LastName.ToUpper()).FirstOrDefault();
            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer Already Exists");
                return StatusCode(422, ModelState);
            }

            var reviewerMap=_mapper.Map<Reviewer>(reviewerCreate);
            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Somthing went to wrong");
                return StatusCode(500, ModelState);
            }
            return StatusCode(201, new { message = "Data saved successfully" });
        }
         [HttpPut("{reviewerId}")]
         [ProducesResponseType(400)]
         [ProducesResponseType(200)]
         [ProducesResponseType(404)]
         public IActionResult UpdateReview(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
         {
             if (updatedReviewer == null)
                 return BadRequest(ModelState);

             if (reviewerId != updatedReviewer.Id)
                 return BadRequest(ModelState);

             if (!_reviewerRepository.ReviewerExists(reviewerId))
                 return NotFound();

             if (!ModelState.IsValid)
                 return BadRequest();

             var reviewMap = _mapper.Map<Reviewer>(updatedReviewer);

             if (!_reviewerRepository.UpdateReviewer(reviewMap))
             {
                 ModelState.AddModelError("", "Something went wrong updating review");
                 return StatusCode(500, ModelState);
             }

            return StatusCode(500, new {message="success"});
        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var review=_reviewerRepository.GetReviewer(reviewerId);
            if (!_reviewerRepository.DeleteReviewer(review))
                return StatusCode(500, "Internal Server error");
            return StatusCode(200, "Data successfully deleted "+ reviewerId);
        }

    }
}
