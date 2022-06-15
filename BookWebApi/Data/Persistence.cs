using BookWebApi.Models;
using System.Text.Json;


namespace BookWebApi.Data;


public class Persistence : IPersistence<Book>, IPersistence<Author>
{
    private const string dataPrefix = "BookApiData_";
    private string dataDirectory = $"{Directory.GetCurrentDirectory()}\\dat\\";
    public void Commit(IRepository<Book> bookRepo)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(
            $"{dataDirectory}{dataPrefix}{DateTimeOffset.Now.ToUnixTimeSeconds()}.dat", 
            JsonSerializer.Serialize(bookRepo.GetAll(),
            options));
    }

    public void Commit(IRepository<Author> authorRepo)
    {
        // Intentional no-op.
    }
}