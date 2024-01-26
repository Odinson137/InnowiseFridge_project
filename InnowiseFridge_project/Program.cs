using InnowiseFridge_project.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


services.AddAutoMapper(typeof(MappingProfile));

services.AddDbContext<DataContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("SqlConnection");
    options.UseSqlServer(connection);
});

services.AddCors(options =>
{
    options.AddPolicy("AllClient", builder =>
    {
        builder.AllowAnyOrigin();
    });
});

services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    await Seed.Seeding(dataContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllClient");

app.Run();
