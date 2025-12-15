using HealthCheck.Server;

// Instantiate a WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

/* Add services to the container */

builder.Services.AddHealthChecks().AddCheck<PingHealthCheck>("Ping");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create a WebApplication object which implements IHost
var app = builder.Build();

/* Configure the HTTP request pipeline by loading the required middleware and services */

app.UseDefaultFiles();
app.UseStaticFiles();

// Use Swagger in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Handle HTTP to HTTPS redirects
app.UseHttpsRedirection();

// Allow requests to be restricted to authorized users
app.UseAuthorization();

// Configure HealthChecks
/* Adding before MapControllers ensures the HealthChecks route won't be overridden
 * by any controller that might share that same route in the future.
 * Learn more about Health Checkes at https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks */
app.UseHealthChecks(new PathString("/api/health"));

// Add the endpoints required by the controller action methods to handle incoming HTTP requests with default routing rules
app.MapControllers();

app.MapFallbackToFile("/index.html");

// Run the app
app.Run();
