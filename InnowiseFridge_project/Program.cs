using System.Text;
using InnowiseFridge_project.Data;
using InnowiseFridge_project.Interfaces.RepositoryInterfaces;
using InnowiseFridge_project.Interfaces.ServiceInterfaces;
using InnowiseFridge_project.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddScoped<IFridge, FridgeRepository>();
services.AddScoped<IFridgeProduct, FridgeProductRepository>();
services.AddScoped<IProduct, ProductRepository>();
services.AddScoped<IUser, UserRepository>();
services.AddScoped<FileService>();
services.AddScoped<ITokenService, TokenService>();

services.AddAutoMapper(typeof(MappingProfile));

services.AddDbContext<DataContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("SqlConnection");
    options.UseSqlServer(connection);
});

services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

services.AddCors(options =>
{
    options.AddPolicy("AllClient", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
    });
});

services.AddControllers(options =>
    {
        options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            return builtInFactory(context);
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        };
    });

services.AddAuthorization(options => 
{ 
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });     
    options.AddPolicy("FridgeOwner", policy =>
    {
        policy.RequireRole("FridgeOwner");
    }); 
});


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

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.UseCors("AllClient");

app.Run();
