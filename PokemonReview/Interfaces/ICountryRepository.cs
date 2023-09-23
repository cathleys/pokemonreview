using PokemonReview.Models;

namespace PokemonReview.Interfaces;
public interface ICountryRepository
{
    //matter of preferrence
    //IEnumerable = barebone where the below types inherit from (polymorphism)
    //ICollection = middle ground *
    //List = most dynamic / full featured
    Task<ICollection<Country>> GetCountries();
    Task<Country> GetCountry(int id);
    Task<Country> GetCountryByOwner(int ownerId);
    //Get Method more done in repository but less in controller
    Task<ICollection<Owner>> GetOwnersByCountry(int countryId);
    bool CountryExists(int id);


    //Create Method toss the whole object to database
    //Also less done in repository but more to done in controller
    Task<bool> AddCountry(Country country);
    Task<bool> UpdateCountry(Country country);
    Task<bool> DeleteCountry(Country country);
    Task<bool> SaveChanges();
}
