using System.ComponentModel.DataAnnotations;
namespace BookWebApi.Contracts;

public class AuthorInfo
{
     [Required(ErrorMessage = "(Wookie)Authors must have a first name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "(Wookie)Authors must have a last name")]
    public string LastName { get; set; }
    
    public string? Pseudonym { get; set; }
}