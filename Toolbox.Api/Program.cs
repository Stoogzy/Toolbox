using Toolbox.Api;
using Toolbox.Application;
using Toolbox.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Application & Infrastructure Services.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Api Services.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
