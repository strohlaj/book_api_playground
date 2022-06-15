using BookWebApi.Contracts;
using BookWebApi.Data;
using BookWebApi.Models;

namespace BookWebApi.Services;

public class RegisterService : IRegisterService
{
    private readonly IRepository<Author> _authorRepo;
    public RegisterService(IRepository<Author> authorRepo)
    {
        _ = authorRepo ?? throw new ArgumentNullException(nameof(authorRepo));
        _authorRepo = authorRepo;
    }

    public bool RegisterUserAsAuthor(RegisterInfo registerInfo)
    {
        _ = registerInfo ?? throw new ArgumentNullException(nameof(registerInfo));

        if (CanRegisterUser(registerInfo))
        {
            return false;
        }
        _authorRepo.Upsert(new Author(
            Guid.NewGuid(),
            registerInfo.AuthorInfo.FirstName,
            registerInfo.AuthorInfo.LastName,
            registerInfo.AuthorInfo.Pseudonym,
            new User(
                registerInfo.UserInfo.Username,
                registerInfo.UserInfo.EmailAddress,
                registerInfo.UserInfo.Password)));
        return true;
    }

    private bool CanRegisterUser(RegisterInfo registerInfo) =>
    _authorRepo.GetAll().Any(a =>
        a.User.EmailAddress.Equals(registerInfo.UserInfo.EmailAddress, StringComparison.CurrentCultureIgnoreCase) ||
    _authorRepo.GetAll().Any(a =>
        a.User.Username.Equals(registerInfo.UserInfo.Username, StringComparison.CurrentCultureIgnoreCase)));


}