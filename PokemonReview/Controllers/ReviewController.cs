using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokemonReview.DTOs;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers;
public class ReviewController : BaseApiController
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public ReviewController(
        IReviewRepository reviewRepository,
        IReviewerRepository reviewerRepository,
        IPokemonRepository pokemonRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _reviewerRepository = reviewerRepository;
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<ICollection<ReviewDto>>> GetReviews()
    {
        var reviews = _mapper.Map<List<ReviewDto>>
        (await _reviewRepository.GetReviews());

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(reviews);
    }


    [HttpGet("{reviewId}")]
    public async Task<ActionResult<ReviewDto>> GetReview(int reviewId)
    {

        var review = _mapper.Map<ReviewDto>
        (await _reviewRepository.GetReview(reviewId));

        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(review);

    }

    [HttpGet("pokemon/{pokeId}")]
    public async Task<ActionResult<ICollection<ReviewDto>>>
    GetReviewsByPokemon(int pokeId)
    {
        var reviews = _mapper.Map<ICollection<ReviewDto>>
        (await _reviewRepository.GetReviewsByPokemon(pokeId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);

    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ReviewDto>> CreateReview(
        [FromQuery] int reviewerId,
        [FromQuery] int pokeId,
        [FromBody] ReviewDto reviewDto)
    {
        if (reviewDto == null) return BadRequest(ModelState);

        var reviews = await _reviewRepository.GetReviews();

        var review = reviews.Where(r => r.Title.Trim().ToUpper()
        == reviewDto.Title.TrimEnd().ToUpper())
        .FirstOrDefault();

        if (review != null)
        {
            ModelState.AddModelError("", "Review already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToReview = _mapper.Map<Review>(reviewDto);

        mapToReview.Reviewer = await _reviewerRepository.GetReviewer(reviewerId);
        mapToReview.Pokemon = await _pokemonRepository.GetPokemonByIdAsync(pokeId);

        if (await _reviewRepository.AddReview(mapToReview))
        {
            return Ok("Review Successfully created");

        }
        else
        {
            ModelState.AddModelError("", "Failed to create a review");
            return StatusCode(500, ModelState);
        }

    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ReviewDto>> UpdateReview(int reviewId,
    [FromBody] ReviewDto reviewDto)
    {
        if (reviewDto == null) return BadRequest(ModelState);

        if (reviewId != reviewDto.Id) return BadRequest("review id doesn't match");

        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound("No review found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToReview = _mapper.Map<Review>(reviewDto);

        if (await _reviewRepository.UpdateReview(mapToReview))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "Failed to update review");
            return StatusCode(500, ModelState);
        }

    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ReviewDto>> DeleteReview(int reviewId)
    {

        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound("No review found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var reviewToDelete = await _reviewRepository.GetReview(reviewId);

        if (await _reviewRepository.DeleteReview(reviewToDelete))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "Failed to delete review");
            return StatusCode(500, ModelState);
        }
    }
}
