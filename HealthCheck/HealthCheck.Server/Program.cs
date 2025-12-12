// Instantiate a WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

// Create a WebApplication object which implements IHost
var app = builder.Build();

#region Configure the HTTP request pipeline by loading the required middleware and services

// The middleware and services added to the HTTP pipeline will process incoming requests in registration order, from top to bottom.

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

// Add the endpoints required by the controller action methods to handle incoming HTTP requests with default routing rules
app.MapControllers();

app.MapFallbackToFile("/index.html");

#endregion

// Run the app
app.Run();
