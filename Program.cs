using Invoicer.Readers;
using Invoicer.Repositories;
using Invoicer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Invoicer.Authentification;
var builder = WebApplication.CreateBuilder(args);
// Add authentication
byte[] key = Encoding.ASCII.GetBytes(builder.Configuration["Keys:AuthenticationKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataBaseConnectionString"))
, ServiceLifetime.Scoped);

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "test_front",
    policy =>
    {
        // Only allow a specific origin for development. Restrict for production.
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Dependency Injection
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<ProviderReader>();
builder.Services.AddScoped<InvoiceReader>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddLogging();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("test_front"); // Properly ordered before auth
app.UseAuthorization();
app.MapControllers();
app.Run();