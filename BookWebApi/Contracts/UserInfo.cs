using System.ComponentModel.DataAnnotations;

namespace BookWebApi.Contracts;
public class UserInfo
{
    [Required(ErrorMessage = "Username required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage ="Password required.")]
    public string? Password { get; set; }

    [Required(ErrorMessage ="Email address required.")]
    public string? EmailAddress { get; set; }
}