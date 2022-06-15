using BookWebApi.Data;
using BookWebApi.Models;
using BookWebApi.Services;
using BookWebApi.Converters;
using BookWebApi.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
.AddControllers()
.AddJsonOptions(options => 
                options.JsonSerializerOptions.Converters.Add(new GuidConverter()))
.AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
    .AddJwtBearer(options =>    
    {    
        options.TokenValidationParameters = new TokenValidationParameters    
        {    
            ValidateIssuer = true,    
            ValidateAudience = true,    
            ValidateLifetime = true,    
            ValidateIssuerSigningKey = true,    
            ValidIssuer = builder.Configuration["Jwt:Issuer"],    
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))    
        };    
    });

var dfw = new Author(
        Guid.NewGuid(), 
        "David", 
        "Wallace", 
        "Wally",
        new User("d-wallace", "david.wallace@outlook.com", "password"));

// var bookRepository = new InMemoryRepository<Book>(new List<Book>(){
//     new Book(Guid.NewGuid(), 
//     "Infinite Jest", 
//     "A big book about sincerity amidst insanity", 
//     dfw, 
//     "img0.jpg", 
//     11.99m, 
//     new DateTime(1996, 02, 01))
// });

// var authorRepository = new InMemoryRepository<Author>(new List<Author>() {
//     dfw
// });

builder.Services.AddSingleton<IPersistence<Book>, Persistence>();
builder.Services.AddSingleton<IPersistence<Author>, Persistence>();
static IEnumerable<Author> getUniqueAuthorsFromBooks(string jsonString)
{
    var books = JsonSerializer.Deserialize<IEnumerable<Book>>(jsonString);
    return books?.Select(b => b.Author).Distinct() ?? Enumerable.Empty<Author>();
}

builder.Services.AddSingleton<IRepository<Book>, InMemoryRepository<Book>>(isp => ModelFactory.Repository<Book>(isp, (json, persistor) => 
    new InMemoryRepository<Book>(
                new List<Book>(JsonSerializer.Deserialize<IEnumerable<Book>>(json) ?? Enumerable.Empty<Book>()),
                persistor)));

builder.Services.AddSingleton<IRepository<Author>, InMemoryRepository<Author>>(isp => ModelFactory.Repository<Author>(isp, (json, persistor) => 
     new InMemoryRepository<Author>(
                new List<Author>(getUniqueAuthorsFromBooks(json)),
                persistor)));


builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomMiddlewares();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


/// <summary>
/// This middleware will intercept an http request and modify the accept header value
/// if the content type is a standard XML content-type as defined in rfc3023.
/// reference:  https://www.rfc-editor.org/rfc/rfc3023#section-3
/// This is to ensure that if the api is given an xml content-type header we will respond 
/// with an xml serialized response. By default web api will serialize the response 
/// based on the accept header but not the content-type header because of this, we can 
/// enforce the serialization response by tricking the pipeline that the accept header 
/// is `application/xml`.
/// </summary>
public class ContentTypeResponseEnforcerMiddleware
{
    private readonly RequestDelegate _next;

    public ContentTypeResponseEnforcerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        if (RequestHeaderHasXmlContentType(httpContext.Request))
        {
            httpContext.Request.Headers.Remove("accept");
            httpContext.Request.Headers.Add("accept", "application/xml");
        }
        return _next(httpContext);
    }
    /// <summary>
    /// finds XML content-type headers as defined in rfc3023. The application/xml content type
    /// header can have encoding information after the value which why we 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private bool RequestHeaderHasXmlContentType(HttpRequest request) =>
        request.Headers.ContentType.Any(s => s.StartsWith("application/xml")) ||
        request.Headers.ContentType.Any(s => s.StartsWith("text/xml"));
}

/// <summary>
/// An extension method for adding custom middleware to the application builder. 
/// </summary>
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ContentTypeResponseEnforcerMiddleware>();
    }
}
