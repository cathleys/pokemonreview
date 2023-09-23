namespace PokemonReview.Models;
public class Owner
{
    public int Id { get; set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }

    public string Gym { get; set; }
    public Country Country { get; set; }
    public ICollection<PokemonOwner> PokemonOwners { get; set; }
}
