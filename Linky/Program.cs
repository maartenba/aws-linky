using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var links = new Dictionary<string, DynamicLink>(StringComparer.OrdinalIgnoreCase);
links.Add("aws", new DynamicLink
{
    ShortIdentifier = "aws",
    WebLink = "https://aws.amazon.com/console/",
    AndroidMobileLink = "https://play.google.com/store/apps/details?id=com.amazon.aws.console.mobile",
    AppleMobileLink = "https://apps.apple.com/us/app/aws-console/id580990573"
});

app.MapGet("/{shortIdentifier}", (string shortIdentifier, HttpContext http) =>
{
    if (links.TryGetValue(shortIdentifier, out var dynamicLink))
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

        http.Response.StatusCode = 301;
        http.Response.Headers.Location = new StringValues(link);
        http.Response.WriteAsync("Redirecting...");
        return;
    }

    http.Response.StatusCode = 404;
    http.Response.WriteAsync("Page not found");
});

app.Run();

public class DynamicLink
{
    public int Id { get; set; }
    public string ShortIdentifier { get; set; }
    public string WebLink { get; set; }
    public string? AndroidMobileLink { get; set; }
    public string? AppleMobileLink { get; set; }
}