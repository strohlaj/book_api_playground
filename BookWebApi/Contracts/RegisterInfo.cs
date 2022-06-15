using System.ComponentModel.DataAnnotations;

namespace BookWebApi.Contracts;

public class RegisterInfo
{
    [Required]
    public AuthorInfo AuthorInfo {get; set;}

    [Required]
    public UserInfo UserInfo {get; set;}
}