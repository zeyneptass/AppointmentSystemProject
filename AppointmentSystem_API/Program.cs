

using AppointmentSystem_Core;
using AppointmentSystem_Domain.Entities.Identity;
using AppointmentSystem_Infrastructure;
using AppointmentSystem_Infrastructure.Persistence.Context;
using AppointmentSystem_Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Infrastructure Services Collection Extension
builder.Services.AddInfrastructureServices(builder.Configuration);
#endregion
#region Core Services Collection Extension
builder.Services.AddCoreServices();
#endregion



#region Jwt Header Configuration

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
           ValidAudience = builder.Configuration["JwtSettings:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
           ClockSkew = TimeSpan.Zero
       };
   });

#endregion
#region JWT Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Appointment System API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

var app = builder.Build();

#region Seed Database
// Uygulama baþlatýldýðýnda bir scope oluþtur , bu sayede servisleri kullanabiliriz
using (var scope = app.Services.CreateScope())
{
    // DI container'dan UserManager<ApplicationUser> servisini al
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    // AdminSeeding sýnýfýndaki SeedAdminAsync metodunu çaðýr
    await AdminSeeding.SeedAdminAsync(userManager);
}
#endregion



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
