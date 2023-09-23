
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.DTOs;
using PokemonReview.Interfaces;
using PokemonReview.Models;


namespace PokemonReview.Controllers;

public class CountryController : BaseApiController
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<CountryDto>>> GetCountries()
    {
        var countries = _mapper.Map<ICollection<CountryDto>>
        (await _countryRepository.GetCountries());

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(countries);
    }


    [HttpGet("{countryId}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int countryId)
    {
        var country = _mapper.Map<CountryDto>
        (await _countryRepository.GetCountry(countryId));

        if (!_countryRepository.CountryExists(countryId))
            return NotFound("country doesn't exist");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(country);

    }

    [HttpGet("owner/{ownerId}")]
    public async Task<ActionResult<CountryDto>> GetCountryByOwner(int ownerId)
    {
        var country = _mapper.Map<CountryDto>
        (await _countryRepository.GetCountryByOwner(ownerId));

        if (!_countryRepository.CountryExists(ownerId))
            return NotFound("country not found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(country);
    }

    [HttpGet("{countryId}/owners")]
    public async Task<ActionResult<ICollection<OwnerDto>>> GetOwnersByCountry(int countryId)
    {
        var owners = _mapper.Map<List<OwnerDto>>
        (await _countryRepository.GetOwnersByCountry(countryId));

        if (!_countryRepository.CountryExists(countryId))
            return NotFound("no owners found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(owners);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CountryDto>> CreateCountry([FromBody] CountryDto countryDto)
    {
        //if user creates nothing.
        if (countryDto == null) return BadRequest(ModelState);


        //grab the countries first then check if there's instance already exists
        var countries = await _countryRepository.GetCountries();

        var country = countries.Where(c => c.Name.Trim().ToUpper()
        == countryDto.Name.TrimEnd().ToUpper())
        .FirstOrDefault();


        if (country != null)
        {
            ModelState.AddModelError("", "Country already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var mapToCountry = _mapper.Map<Country>(countryDto);

        if (await _countryRepository.AddCountry(mapToCountry))
        {

            return Ok("Country Successfully Created");
        }
        else
        {
            ModelState.AddModelError("", "Failed to create country");
            return StatusCode(500, ModelState);
        }


    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CountryDto>> UpdateCountry(
        int countryId,
        [FromBody] CountryDto countryDto
        )
    {
        if (countryDto == null) return BadRequest(ModelState);

        if (countryId != countryDto.Id)
            return BadRequest("country doesn't match");

        if (!_countryRepository.CountryExists(countryId))
            return NotFound("No country found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToCountry = _mapper.Map<Country>(countryDto);

        if (await _countryRepository.UpdateCountry(mapToCountry))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "Failed to update country");
            return StatusCode(500, ModelState);
        }
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CountryDto>> DeleteCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
            return NotFound("country not found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var countryToDelete = await _countryRepository.GetCountry(countryId);
        if (await _countryRepository.DeleteCountry(countryToDelete))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "failed to delete country");
            return StatusCode(500, ModelState);
        }
    }

}
