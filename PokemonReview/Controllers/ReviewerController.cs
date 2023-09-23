using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.DTOs;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers;
public class ReviewerController : BaseApiController
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository,
    IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<ReviewerDto>>> GetReviewers()
    {
        var reviewers = _mapper.Map<List<ReviewerDto>>
        (await _reviewerRepository.GetReviewers());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviewers);
    }

    [HttpGet("{reviewerId}")]
    public async Task<ActionResult<ReviewerDto>> GetReviewer(int reviewerId)
    {
        var reviewer = _mapper.Map<ReviewerDto>
        (await _reviewerRepository.GetReviewer(reviewerId));

        if (!_reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviewer);

    }

    [HttpGet("{reviewerId}/reviews")]
    public async Task<ActionResult<ICollection<ReviewDto>>> GetReviewsByReviewer(int reviewerId)
    {
        var reviews = _mapper.Map<List<ReviewDto>>
        (await _reviewerRepository.GetReviewsByReviewer(reviewerId));

        if (!_reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]

    public async Task<ActionResult<ReviewerDto>> CreateReviewer(
        [FromBody] ReviewerDto reviewerDto)
    {
        if (reviewerDto == null) return BadRequest(ModelState);

        var reviewers = await _reviewerRepository.GetReviewers();

        var reviewer = reviewers.Where(r => r.LastName.Trim().ToUpper()
        == reviewerDto.LastName.TrimEnd().ToUpper())
        .FirstOrDefault();


        if (reviewer != null)
        {
            ModelState.AddModelError("", "Reviewer already exists");
            return StatusCode(422, ModelState);
        }

        var mapToReviewer = _mapper.Map<Reviewer>(reviewerDto);

        if (await _reviewerRepository.AddReviewer(mapToReviewer))
        {
            return Ok("Reviewer successfully created");
        }
        else
        {
            ModelState.AddModelError("", "Failed to create reviewer");
            return StatusCode(500, ModelState);
        }
    }

    [HttpPut("{reviewerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ReviewerDto>> UpdateReviewer(
        int reviewerId,
        [FromBody] ReviewerDto reviewerDto
        )
    {
        if (reviewerDto == null) return BadRequest(ModelState);

        if (reviewerId != reviewerDto.Id)
            return BadRequest("reviewer id doesn't match");

        if (!_reviewerRepository.ReviewerExists(reviewerId))
            return NotFound("no reviewer exists");


        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToReviewer = _mapper.Map<Reviewer>(reviewerDto);

        if (await _reviewerRepository.UpdateReviewer(mapToReviewer))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "failed to update reviewer");
            return StatusCode(500, ModelState);
        }
    }

    [HttpDelete("{reviewerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ReviewerDto>> DeleteReviewer(int reviewerId)
    {
        if (!_reviewerRepository.ReviewerExists(reviewerId))
            return NotFound("no reviewer exists");


        if (!ModelState.IsValid) return BadRequest(ModelState);

        var reviewerToDelete = await _reviewerRepository.GetReviewer(reviewerId);

        if (await _reviewerRepository.DeleteReviewer(reviewerToDelete))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "Failed to delete reviewer");
            return StatusCode(500, ModelState);
        }
    }
}


