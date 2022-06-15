using BookWebApi.Contracts;
using BookWebApi.Data;
using BookWebApi.Models;

namespace BookWebApi.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepository<Author> _authorRepo;
    public AuthenticationService(IRepository<Author> authorRepo)
    {
        _ = authorRepo ?? throw new ArgumentNullException(nameof(authorRepo));
        _authorRepo = authorRepo;
    }
    public (User?, Guid) AuthenticateUser(UserInfo userInfo)
    {
        _ = userInfo ?? throw new ArgumentNullException(nameof(userInfo));
        return _authorRepo
            .GetAll()
            .Where(a => 
                a.User.EmailAddress == userInfo.EmailAddress &&
                a.User.Username == userInfo.Username && 
                a.User.Password == userInfo.Password)
            .Select(a => (a.User, a.GetId()))
            .FirstOrDefault();
    }
}