
namespace BookWebApi.QueryParameters;

public class BookQueryParameters
{
    public string Title { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public bool IsEmptyQueryParameter =>
        Title == string.Empty && FirstName == string.Empty && LastName == string.Empty;
}