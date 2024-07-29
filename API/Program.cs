using LayeredAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

// Registering controllers to handle incoming HTTP requests
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Registering authorization services to handle role-based or policy-based authorization
builder.Services.AddAuthorization();

// Add Swagger services
builder.Services.AddSwaggerConfigurations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

app.UseHttpsRedirection();

// Adding authentication and authorization middleware to the pipeline
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();