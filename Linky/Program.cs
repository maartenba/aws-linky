using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

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

app.MapGet("/{shortIdentifier}", async Task<Results<NotFound, RedirectHttpResult>>(string shortIdentifier, HttpContext http) =>
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

        return TypedResults.Redirect(link, permanent: false);
    }

    return TypedResults.NotFound();
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