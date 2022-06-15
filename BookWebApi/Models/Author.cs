using System.Runtime.Serialization;

namespace BookWebApi.Models;

/// <summary>
/// Use Datacontract to prevent user model from being serialized on the way out
/// </summary>
[DataContract]
public class Author : IBaseModel
{
    private readonly Guid _id;
    public Guid GetId() => _id;

    private string _firstname;

    [DataMember]
    public string FirstName
    {
        get => _firstname;
        set =>_firstname = value;
    }

    private string _lastname;

    [DataMember]
    public string LastName
    {
        get => _lastname;
        set => _lastname = value;
    }
    
    private string _pseudonym;

    [DataMember]
    public string Pseudonym
    {
        get => _pseudonym;
        set => _pseudonym = value;
    }

    User _user;

    // Explicitly lacking a [DataMember] so as to prevent serialization of this member.
    public User User
    {
        get => _user;
        init => _user = value;
    }

    /// <summary>
    /// Intentional empty constructor, required for xml serializer
    /// </summary>  
    public Author()
    {
        // intentional no-op
    }
    public Author(Guid id, string firstname, string lastname, string pseudonym, User user)
    {
        _id = id;
        _firstname = firstname;
        _lastname = lastname;
        _pseudonym = pseudonym;
        _user = user;
    }
}