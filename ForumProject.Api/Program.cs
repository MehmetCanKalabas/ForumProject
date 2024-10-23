using FluentValidation;
using ForumProjects.Application.Services;
using ForumProjects.Infrastructure.Data;
using ForumProjects.Infrastructure.DTOs;
using ForumProjects.Infrastructure.DTOs.AccountDTOs;
using ForumProjects.Infrastructure.Entities;
using ForumProjects.Infrastructure.FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext ayarlarý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity ayarlarý
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// FluentValidation ayarlarý
builder.Services.AddScoped<IValidator<AccountCreateDTO>, AccountDTOValidator>();
builder.Services.AddScoped<UserService>();

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

app.Run();
