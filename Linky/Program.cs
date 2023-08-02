using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddDbContext<LinkyDbContext>(options => options
    .UseMySql(builder.Configuration.GetConnectionString("db"), new MySqlServerVersion(new Version(5, 7, 12))));

var app = builder.Build();

app.MapGet("/{shortIdentifier}", async Task<Results<NotFound, RedirectHttpResult>>(string shortIdentifier, HttpContext http, LinkyDbContext db) =>
{
    var dynamicLink = await db.Links.FirstOrDefaultAsync(it => it.ShortIdentifier == shortIdentifier.ToLowerInvariant());
    if (dynamicLink != null)
    {
        var link = dynamicLink.WebLink;

        var userAgent = http.Request.Headers.UserAgent.ToString();
        if (!string.IsNullOrEmpty(userAgent) && userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
        {
            link = dynamicLink.AndroidMobileLink;
        }
        else if (!string.IsNullOrEmpty(userAgent) && (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase) || userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase)))
        {
            link = dynamicLink.AppleMobileLink;
        }

        return TypedResults.Redirect(link, permanent: false);
    }

    return TypedResults.NotFound();
});

app.Run();

public class DynamicLink
{
    [Key]
    public int Id { get; set; }

    [MaxLength(10)]
    public string ShortIdentifier { get; set; }

    [MaxLength(400)]
    public string WebLink { get; set; }

    [MaxLength(400)]
    public string? AndroidMobileLink { get; set; }

    [MaxLength(400)]
    public string? AppleMobileLink { get; set; }
}

public class LinkyDbContext : DbContext
{
    public DbSet<DynamicLink> Links { get; set; }

    public LinkyDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DynamicLink>()
            .HasIndex(it => it.ShortIdentifier);

        modelBuilder.Entity<DynamicLink>()
            .HasData(new DynamicLink
            {
                Id = 1,
                ShortIdentifier = "aws",
                WebLink = "https://aws.amazon.com/console/",
                AndroidMobileLink = "https://play.google.com/store/apps/details?id=com.amazon.aws.console.mobile",
                AppleMobileLink = "https://apps.apple.com/us/app/aws-console/id580990573"
            });
    }
}