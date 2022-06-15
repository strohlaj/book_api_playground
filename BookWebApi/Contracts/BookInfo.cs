using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BookWebApi.Converters;

namespace BookWebApi.Contracts;

public class BookInfo
{
    [JsonConverter(typeof(GuidConverter))]
    public Guid Id { get; set; } = Guid.Empty;

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }
    public string? ImageName { get; set; }
    public byte[]? Image { get; set; }

    public string? AuthorName {get; set; }
}


