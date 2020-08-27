## How to Install?
Install-Package SitemapCore


## How to Use?
### In Startup.cs

For basic configuration
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddSitemap(options =>
    {
        options.BaseUrl = "http://localhost/"; // Required, your domain url goes here
    });

    services.AddControllersWithViews();
}
```

For advanced configuration
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddSitemap(options =>
    {
        options.BaseUrl = "http://localhost/"; // Required, your domain url goes here
        options.SitemapPath = "/sitemap"; // Optionally
        options.ForUserAgent("*").Allow("/"); // Optionally, fluent api to configure robots.txt
        options.SetDynamicProvider<MyCustomSitemapProvider>(); // Optionally, for dynamic urls
    });

    services.AddControllersWithViews();
}

public class MyCustomSitemapProvider : ISitemapProvider
{
    public MyCustomSitemapProvider(ILocationHelper locationHelper)
    {
        _locationHelper = locationHelper;
    }

    private readonly ILocationHelper _locationHelper;

    // Your logic goes here
    public Task<IEnumerable<Sitemap>> InvokeAsync()
    {
        var location = _locationHelper.GetSitemapUrl("example-path");
        var myMock = new Sitemap(location)
        {
            ChangeFrequency = ChangeFrequency.Monthly,
            Priority = SitemapPriority.AboveNormal,
        };

        return Task.FromResult(Enumerable.Repeat(myMock, 3));
    }
}
```

...and finally, use the middleware
```
public void Configure(IApplicationBuilder app)
{
    app.UseSitemap(); // Required
    app.UseStaticFiles();

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}
```

### In your Controllers
Add SitemapAttribute to actions that you want to map
```
[Sitemap(Priority = SitemapPriority.Highest, ChangeFrequency = ChangeFrequency.Weekly, LastModification = "2020-08-25")]
public IActionResult Index()
{
    return View();
}
```
