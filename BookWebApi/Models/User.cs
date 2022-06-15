namespace BookWebApi.Models;

/// <summary>
/// This record should only be serialized and displayable at admin-level endpoints - it will not 
/// be xml serializable because it lacks a parameterless constructor. Explicitely does not inherit from 
/// IBaseModel because all Authors are users inherently.  
/// </summary>
/// <param name="Username"></param>
/// <param name="EmailAddress"></param>
/// <param name="Password"></param>
/// <returns></returns>
public record User(string Username, string EmailAddress, string Password);