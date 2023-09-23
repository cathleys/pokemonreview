using AutoMapper;
using PokemonReview.DTOs;
using PokemonReview.Models;

namespace PokemonReview.Helpers;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Pokemon, PokemonDto>();
        CreateMap<PokemonDto, Pokemon>();

        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();

        CreateMap<Country, CountryDto>();
        CreateMap<CountryDto, Country>();

        CreateMap<Owner, OwnerDto>();
        CreateMap<OwnerDto, Owner>();

        CreateMap<Review, ReviewDto>();
        CreateMap<ReviewDto, Review>();

        CreateMap<Reviewer, ReviewerDto>();
        CreateMap<ReviewerDto, Reviewer>();

    }

}
