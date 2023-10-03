
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Controllers;
using PokemonReview.DTOs;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Tests.Controllers
{
    public class PokemonControllerTests
    {

        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private PokemonController _pokemonController;
       
        
        public PokemonControllerTests()
        {
            //Dependencies

            _pokemonRepository = A.Fake<IPokemonRepository>();
            _reviewRepository = A.Fake<IReviewRepository>();
            _mapper = A.Fake<IMapper>();

            //SUT
            _pokemonController = new PokemonController(
                _pokemonRepository, _reviewRepository, _mapper
                );
        }

        [Fact]
        public void PokemonController_GetPokemons_ReturnsSuccess()
        {
            //Arrange
            var pokemons = A.Fake<IEnumerable<Pokemon>>();
            var pokemonList = A.Fake<List<PokemonDto>>();
            A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemons)).Returns(pokemonList);
           
            //Act
            var result = _pokemonController.GetPokemons();
            
            //Assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(Task<ActionResult<IEnumerable<PokemonDto>>>));

        }

        [Fact]
        public async void PokemonController_CreatePokemon_ReturnsSuccess()
        {
            //Arrange
            var ownerId = 1;
            var categoryId = 2;
            var pokemonDto = A.Fake<PokemonDto>();
            var pokemon = A.Fake<Pokemon>();
            var pokemonList = new List<Pokemon>();



            A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonDto)).Returns(pokemon);
                
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonDto)).Returns(pokemon);
            A.CallTo(() => _pokemonRepository.AddPokemon(ownerId, categoryId, pokemon)).Returns(true);
            //Act

            var result = await _pokemonController.CreatePokemon(ownerId, categoryId, pokemonDto);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<PokemonDto>));
        }
    }
}
