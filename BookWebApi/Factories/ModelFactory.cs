using BookWebApi.Data;
using BookWebApi.Models;

namespace BookWebApi.Factories;

public static class ModelFactory
{
    private static string dataDirectory = $"{Directory.GetCurrentDirectory()}\\dat";
    private const string dataPrefix = "BookApiData_";
    private static string? latestFile;
    static ModelFactory()
    {
        Directory.CreateDirectory(dataDirectory);
        int fileStartIndex = dataDirectory.Length + dataPrefix.Length + 1;
        int fileExtensionIndex = 4;
        latestFile = Directory.EnumerateFiles(dataDirectory, "*.dat")
             .OrderByDescending(s => Int64.Parse(s[fileStartIndex..^fileExtensionIndex]))
             .FirstOrDefault();
    }

    public static InMemoryRepository<T> Repository<T>(
        IServiceProvider serviceProvider, 
        Func<string, IPersistence<T>, InMemoryRepository<T>> builder) where T : IBaseModel
    {
        var persistor = serviceProvider.GetService<IPersistence<T>>();
        _ = persistor ?? throw new ArgumentException("Persistor required to build in memory repository");
        if (latestFile is not null)
        {
            var jsonString = File.ReadAllText(latestFile);
            return builder(jsonString, persistor);
        }
        else
        {
            return new InMemoryRepository<T>(new List<T>(), persistor);
        }
    }
}