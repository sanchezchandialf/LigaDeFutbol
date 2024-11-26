using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using LigaDeFutbol.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var secret = builder.Configuration["settings:secret"];
var key = Encoding.ASCII.GetBytes(secret);


builder.Services.AddAuthentication(config=>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config => { 
   config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
});

var app = builder.Build();
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
