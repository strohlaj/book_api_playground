namespace BookWebApi.Models;
public class Book : IBaseModel
{

    private readonly Guid _id;
    public Guid GetId() => _id;

    private string _title;
    public string Title
    {
        get => _title;
        set => _title = value;
    }
    
    private string _description;
    public string Description
    {
        get => _description;
        set => _description = value;
    }
    
    private Author _author;
    public Author Author
    {
        get => _author;
        set => _author = value;
    }
    
    private string _image;
    public string Image
    {
        get =>_image; 
        set => _image = value;
    }

    private decimal _price;
    public decimal Price
    {
        get => _price; 
        set => _price = value; 
    }

    private DateTime _publishedDate;
    public DateTime PublishedDate
    {
        get => _publishedDate; 
        set => _publishedDate = value; 
    }
    

    /// <summary>
    /// Intentional empty constructor, required for xml serializer
    /// </summary>  
    public Book()
    {
        // Intentinoal no-op
    }
    public Book(
        Guid id, 
        string title, 
        string description, 
        Author author, 
        string image, 
        decimal price, 
        DateTime publishedDate)
    {
        _id = id == Guid.Empty ? Guid.NewGuid() : id;
        _title = title;
        _description = description;
        _author = author;
        _image = image;
        _price = price;
        _publishedDate = publishedDate;
    }
}