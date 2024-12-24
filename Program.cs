using EmployeeManagerAPI.Services; // Importing the namespace for in-memory data service.
using Microsoft.AspNetCore.Diagnostics; // Required for exception handling features.
using Serilog; // Required for logging functionalities using Serilog.

var builder = WebApplication.CreateBuilder(args);

// Step 1: Configure Serilog for logging
// Configure Serilog to log messages to both the console and a file with daily rolling intervals.
Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

// Step 2: Add logging to the DI container (dependency injection)
// Integrate Serilog into the application's logging infrastructure.
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog(); // Use Serilog as the logging provider.
});

// Step 3: Add required services to the DI container
builder.Services.AddEndpointsApiExplorer(); // Enables API endpoint discovery for Swagger/OpenAPI.
builder.Services.AddSwaggerGen(); // Adds support for generating Swagger documentation.
builder.Services.AddControllers(); // Adds support for MVC controllers.

// Register DataService as a Singleton (since it uses in-memory data storage for simplicity).
builder.Services.AddSingleton<InMemoryData>();

// Step 4: Configure CORS (Cross-Origin Resource Sharing)
// Define a policy to allow requests from the frontend's localhost URL.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Replace with the URL of your frontend.
               .AllowAnyHeader() // Allow all headers.
               .AllowAnyMethod(); // Allow all HTTP methods.
    });
});

var app = builder.Build();

// Step 5: Middleware to log all incoming HTTP requests
// Logs the HTTP method, URL, and time for every request.
app.Use(async (context, next) =>
{
    Log.Information("HTTP Request: {Method} {Url} at {Time}", context.Request.Method, context.Request.Path, DateTime.Now);
    await next.Invoke();
});

// Step 6: Global error handling middleware
// Catches and logs unhandled exceptions and returns a generic error response.
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            Log.Error(exception, "Unhandled exception occurred at {Time}", DateTime.Now);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError; // Set HTTP status code to 500.
            await context.Response.WriteAsync("An error occurred while processing your request."); // Send error response to client.
        }
    });
});

// Step 7: Apply CORS middleware
// Enables CORS policy defined earlier to allow frontend requests.
app.UseCors("AllowSpecificOrigins");

// Step 8: Configure middleware for development environment
// Adds Swagger UI and API documentation for easier API testing during development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Step 9: Add HTTPS redirection middleware
// Ensures that all HTTP requests are redirected to HTTPS for secure communication.
app.UseHttpsRedirection();

// Step 10: Map controllers
// Adds route mapping for controllers defined in the application.
app.MapControllers();

// Example endpoint: Weather forecast API
// A simple demo endpoint that returns a random weather forecast.
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast") // Sets a unique name for the endpoint.
.WithOpenApi();  // Adds OpenAPI documentation for the endpoint.

// Step 11: Log application startup
// Logs a message indicating that the application is running.
Log.Information("Application is running.");

// Start the application
app.Run();

// Record type: WeatherForecast
// Defines the structure of the weather forecast data returned by the API.
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); // Converts Celsius to Fahrenheit.
}
