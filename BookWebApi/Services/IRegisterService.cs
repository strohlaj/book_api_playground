using BookWebApi.Contracts;

namespace BookWebApi.Services;

public interface IRegisterService
{
    /// <summary>
    /// Uses the registration info to create authors and users within the repository.
    /// </summary>
    /// <param name="registerInfo"></param>
    /// <returns>true - registration was succesful, false - registration failed </returns>
    public bool RegisterUserAsAuthor(RegisterInfo registerInfo);
}