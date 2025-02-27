var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
var connection = builder.Configuration.GetConnectionString("Database");

// Add services to the container.

// Add MediatR Services
builder.Services.AddMediatR(config =>
{
  config.RegisterServicesFromAssembly(assembly);
  config.AddOpenBehavior(typeof(ValidationBehavior<,>));
  config.AddOpenBehavior(typeof(LogginBehavior<,>));
});

// add Validators Services
builder.Services.AddValidatorsFromAssembly(assembly);

// Add Carter Services to the container.
builder.Services.AddCarter();

// Add Marten Services
builder.Services.AddMarten(opts =>
{
  opts.Connection(connection!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
  builder.Services.InitializeMartenWith<CatalogInitialData>();
}

// Add Custom Exception Handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks().AddNpgSql(connection!);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

// Configure Exception Handler
app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
  new HealthCheckOptions
  {
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
  });

app.Run();
