using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.DTOs;
using PokemonReview.Models;
using PokemonReview.Repositories;

namespace PokemonReview.Controllers;

public class CategoryController : BaseApiController
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = _mapper.Map<IEnumerable<CategoryDto>>
        (await _categoryRepository.GetCategories());

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int categoryId)
    {
        var category = _mapper.Map<CategoryDto>
        (await _categoryRepository.GetCategory(categoryId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound("category doesn't exist");

        return Ok(category);
    }

    [HttpGet("pokemon/{categoryId}")]
    public async Task<ActionResult<ICollection<PokemonDto>>>
    GetPokemonByCategory(int categoryId)
    {
        var pokemons = _mapper.Map<ICollection<PokemonDto>>
        (await _categoryRepository.GetPokemonByCategory(categoryId));

        if (!ModelState.IsValid) return BadRequest(ModelState);

        return Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CategoryDto>> CreateCategory
    ([FromBody] CategoryDto categoryDto)
    {
        if (categoryDto == null) return BadRequest(ModelState); // if the user creates nothing

        //next, Check if there's object exists already
        var categories = await _categoryRepository.GetCategories();

        var category = categories
                .Where(c => c.Name.Trim().ToUpper()
                == categoryDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

        if (category != null)
        {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToCategory = _mapper.Map<Category>(categoryDto);


        if (await _categoryRepository.AddCategory(mapToCategory))
        {

            return Ok("Successfully created");
        }
        else
        {
            ModelState.AddModelError("", "Failed to create category");
            return StatusCode(500, ModelState);
        }


    }
    [HttpPut("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]

    public async Task<ActionResult<CategoryDto>> UpdateCategory(
    int categoryId,
    [FromBody] CategoryDto categoryDto)
    {
        if (categoryDto == null) return BadRequest(ModelState);

        if (categoryId != categoryDto.Id)
            return BadRequest("category id doesn't match");

        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound("category not found");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var mapToCategory = _mapper.Map<Category>(categoryDto);


        if (await _categoryRepository.UpdateCategory(mapToCategory))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "Failed to update category");
            return StatusCode(500, ModelState);
        }
    }

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<CategoryDto>> DeleteCategory(
        int categoryId)
    {

        if (!_categoryRepository.CategoryExists(categoryId))
            return NotFound("category not found");

        if (!ModelState.IsValid) return BadRequest(ModelState);


        var categoryToDelete = await _categoryRepository.GetCategory(categoryId);

        if (await _categoryRepository.DeleteCategory(categoryToDelete))
        {
            return NoContent();
        }
        else
        {
            ModelState.AddModelError("", "failed to delete category");
            return StatusCode(500, ModelState);
        }
    }

}