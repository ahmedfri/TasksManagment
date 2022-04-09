using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Contracts;
using Repository;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<DatabaseContext>()
.AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
    };
});
 builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter ‘Bearer’ [space] then your valid token from the login response.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJytfdseesersdttdaDFXDXZWETEESDDX9\"",
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
                     new string[] {}
                     }
                     });
});

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString")));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


var app = builder.Build();
    if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
    }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
