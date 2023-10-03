
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Models;
using PokemonReview.Repositories;

namespace PokemonReview.Tests.Repositories
{
    public class PokemonRepositoryTests
    {


        private async Task<DataContext> GetDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dataContext = new DataContext(options);

            dataContext.Database.EnsureCreated();
            if (await dataContext.Pokemons.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    dataContext.Pokemons.Add(
                        new Pokemon()
                        {
                            Name = "Pikachu",
                            BirthDate = new DateTime(1903, 1, 1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Electric"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Pikachu",Text = "Pickahu is the best pokemon, because it is electric", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title="Pikachu", Text = "Pickachu is the best a killing rocks", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Pikachu",Text = "Pickchu, pickachu, pikachu", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        });
                    await dataContext.SaveChangesAsync();
                }
            }
            return dataContext;

        }

        [Fact]
        public async void PokemonRepository_GetPokemon_ReturnsPokemon()
        {
            //Arrange
            var name = "Pikachu";

            var dataContext = await GetDataContext();
            var pokemonRepository = new PokemonRepository(dataContext);
            //Act
            var result = pokemonRepository.GetPokemonByNameAsync(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Pokemon>>();
        }

        [Fact]
        public async void PokemonController_GetPokemonByRating_ReturnsDecimalBetween1To10()
            {
            //Arrange
            var pokeId = 1;
            var dataContext = await GetDataContext();
            var pokemonRepository = new PokemonRepository(dataContext);
            //Act
            var result = pokemonRepository.GetPokemonByRatingAsync(pokeId);
            //Assert
            result.Should().NotBe(0);
            result.Should().BeInRange(1,10);

            }

    }
}
