using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Northwind.EntityModels;

#region Configure web server host and services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddNorthwindContext();
var app = builder.Build();
#endregion

#region Configure the HTTP pipeline and routes
if (!app.Environment.IsDevelopment())
{
  app.UseHsts();
}
app.UseHttpsRedirection();
app.Use(async (HttpContext context, Func<Task> next) =>
{
  RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;
  if (rep is not null)
  {
    WriteLine($"Endpoint name: {rep.DisplayName}");
    WriteLine($"Endpoint route pattern: {rep.RoutePattern.RawText}");
  }
  if (context.Request.Path == "/bonjour")
  {
    // In the case of a match on URL path, this becomes a terminating
    // delegate that returns so does not call the next delegate.
    await context.Response.WriteAsync("Bonjour Monde!");
    return;
  }
  await next();
  // We could modify the response after calling the next delegate.
});
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapRazorPages();
app.MapGet("/hello", () => 
  $"Environment is {app.Environment.EnvironmentName}");
#endregion

// Start the web server, host the website, and wait for requests.
app.Run(); // This is a thread-blocking call.
WriteLine("This executes after the web server has stopped!");
