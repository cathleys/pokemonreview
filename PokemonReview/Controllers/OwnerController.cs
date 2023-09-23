using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.DTOs;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers;
public class OwnerController : BaseApiController
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly ICountryRepository _countryRepository;

    public OwnerController(IOwnerRepository ownerRepository,
    IMapper mapper, ICountryRepository countryRepository)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _countryRepository = countryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<OwnerDto>>> GetOwners()
    {
        var owners = _mapper.Map<List<OwnerDto>>
        (await _ownerRepository.GetOwners());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owners);
    }
    [HttpGet("{ownerId}")]
    public async Task<ActionResult<OwnerDto>> GetOwner(int ownerId)
    {
        var owner = _mapper.Map<OwnerDto>(await _ownerRepository.GetOwner(ownerId));

        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound("owner doesn't exist");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owner);
    }


    [HttpGet("{pokeId}/owners")]
    public async Task<ActionResult<ICollection<OwnerDto>>> GetOwnersByPokemon(int pokeId)
    {
        var owners = _mapper.Map<List<OwnerDto>>
        (await _ownerRepository.GetOwnersByPokemon(pokeId));


        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpGet("{ownerId}/pokemon")]
    public async Task<ActionResult<Owner>> GetPokemonByOwner(int ownerId)
    {
        var pokemons = _mapper.Map<List<PokemonDto>>
        (await _ownerRepository.GetPokemonByOwner(ownerId));

        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound("no owner exists");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemons);
    }


    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<OwnerDto>> CreateOwner(
        [FromQuery] int countryId,
        [FromBody] OwnerDto ownerDto)
    {
        if (ownerDto == null) return BadRequest(ModelState);

        var owners = await _ownerRepository.GetOwners();

        var owner = owners
        .Where(o => o.LastName.Trim().ToUpper()
         == ownerDto.LastName.TrimEnd().ToUpper())
        .FirstOrDefault();

        if (owner != null)
        {
            ModelState.AddModelError("", "Owner already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToOwner = _mapper.Map<Owner>(ownerDto);
        mapToOwner.Country = await _countryRepository.GetCountry(countryId);

        if (await _ownerRepository.AddOwner(mapToOwner))
        {

            return Ok("Owner Successfully created");
        }
        else
        {
            ModelState.AddModelError("", "Failed to create owner");
            return StatusCode(500, ModelState);
        }

    }

    [HttpPut("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<OwnerDto>> UpdateOwner(
        int ownerId, [FromBody] OwnerDto ownerDto)
    {
        if (ownerDto == null) return BadRequest(ModelState);

        if (ownerId != ownerDto.Id)
            return BadRequest("Owner id doesn't match");

        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound("Owner doesn't exist");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToOwner = _mapper.Map<Owner>(ownerDto);

        if (await _ownerRepository.UpdateOwner(mapToOwner))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "Failed to update owner");
            return StatusCode(500, ModelState);
        }
    }

    [HttpDelete("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<OwnerDto>> DeleteOwner(int ownerId)
    {
        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound("owner doesn't exists");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ownerToDelete = await _ownerRepository.GetOwner(ownerId);

        if (await _ownerRepository.DeleteOwner(ownerToDelete))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "failed to delete owner");
            return StatusCode(500, ModelState);
        }
    }

}
