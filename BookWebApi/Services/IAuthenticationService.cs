using BookWebApi.Contracts;
using BookWebApi.Models;

namespace BookWebApi.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// Uses the userinfo to authenticate the user against the users in the repository.
    /// </summary>
    /// <param name="userInfo"></param>
    /// <returns>A tuple of the user and the id that maps to the user id</returns>
    public (User?, Guid) AuthenticateUser(UserInfo userInfo);
}