using Ecommerce.Shared.Contract.Extensions;
using Ecommerce.Shared.DataAccess.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Register Database services
builder.Services.AddDatabase(
    builder.Configuration,
    Assembly.GetExecutingAssembly(), // get assembly containing migrations
    "Database" // Section name in appsettings.json
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    // Enable Swagger middleware
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1");
        c.RoutePrefix = "swagger"; // Access Swagger UI at /swagger
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();

