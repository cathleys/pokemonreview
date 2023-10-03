using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.DTOs;
using PokemonReview.Interfaces;
using PokemonReview.Models;


namespace PokemonReview.Controllers;


public class PokemonController : BaseApiController
{
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(
        IPokemonRepository pokemonRepository,
        IReviewRepository reviewRepository,
        IMapper mapper)
        {
                _pokemonRepository = pokemonRepository;
                _reviewRepository = reviewRepository;
                _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonDto>>> GetPokemons()
        {
                var pokemons = _mapper.Map<IEnumerable<PokemonDto>>
                (await _pokemonRepository.GetPokemonsAsync());

                if (!ModelState.IsValid) return BadRequest(ModelState);

                return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        public async Task<ActionResult<PokemonDto>> GetPokemonById(int pokeId)
        {
                var pokemon = _mapper.Map<PokemonDto>
                 (await _pokemonRepository.GetPokemonByIdAsync(pokeId));

                if (!_pokemonRepository.PokemonExist(pokeId))
                {
                        return NotFound("pokemon does not exist");
                }

                if (!ModelState.IsValid) return BadRequest(ModelState);

                return Ok(pokemon);

        }

        // [HttpGet("{name}")]
        // public async Task<ActionResult<PokemonDto>> GetPokemonByName(string name)
        // {
        //         var pokemon = _mapper.Map<PokemonDto>
        //         (await _pokemonRepository.GetPokemonByNameAsync(name));



        //         if (!ModelState.IsValid) return BadRequest(ModelState);

        //         return Ok(pokemon);
        // }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByRating(int pokeId)
        {
                if (!_pokemonRepository.PokemonExist(pokeId))
                        return NotFound();

                var rating = _pokemonRepository.GetPokemonByRatingAsync(pokeId);

                if (!ModelState.IsValid) return BadRequest(ModelState);

                return Ok(rating);
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PokemonDto>> CreatePokemon(
                [FromQuery] int ownerId,
                [FromQuery] int categoryId,
                [FromBody] PokemonDto pokemonDto)
        {
                if (pokemonDto == null) return BadRequest(ModelState);

                var pokemons = await _pokemonRepository.GetPokemonTrimToUpper(pokemonDto);


                if (pokemons != null)
                {
                        ModelState.AddModelError("", "Pokemon already exists");
                        return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid) return BadRequest(ModelState);

                var mapToPokemon = _mapper.Map<Pokemon>(pokemonDto);


                if (await _pokemonRepository.AddPokemon(
                        ownerId, categoryId, mapToPokemon))
                {

                        return Ok("Pokemon successfully created");
                }
                else
                {
                        ModelState.AddModelError("", "Failed to create pokemon");
                        return StatusCode(500, ModelState);
                }

        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PokemonDto>> UpdatePokemon(
                [FromQuery] int ownerId,
                [FromQuery] int categoryId,
                int pokeId,
                [FromBody] PokemonDto pokemonDto)
        {
                if (pokemonDto == null) return BadRequest(ModelState);

                if (pokeId != pokemonDto.Id)
                        return BadRequest("pokemon id doesn't match");

                if (!_pokemonRepository.PokemonExist(pokeId))
                        return NotFound("No pokemon found");

                if (!ModelState.IsValid) return BadRequest(ModelState);

                var mapToPokemon = _mapper.Map<Pokemon>(pokemonDto);

                if (await _pokemonRepository.UpdatePokemon(
                        ownerId, categoryId, mapToPokemon))
                {
                        return NoContent();

                }
                else
                {
                        ModelState.AddModelError("", "Failed to update owner");
                        return StatusCode(500, ModelState);
                }
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PokemonDto>> DeletePokemon(int pokeId)
        {
                if (!_pokemonRepository.PokemonExist(pokeId))
                        return NotFound("No pokemon found");


                var pokemonToDelete = await _pokemonRepository
                .GetPokemonByIdAsync(pokeId);

                if (!ModelState.IsValid) return BadRequest(ModelState);

                var reviewsToDelete = await _reviewRepository
                .GetReviewsByPokemon(pokeId);

                if (await _reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
                {
                        return NoContent();

                }

                if (await _pokemonRepository.DeletePokemon(pokemonToDelete))
                {
                        return NoContent();
                }
                else
                {
                        ModelState.AddModelError("", "failed to delete pokemon");
                        return StatusCode(500, ModelState);
                }


        }

}
