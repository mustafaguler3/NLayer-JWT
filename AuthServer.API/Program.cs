using CoreLayer.Configuration;
using CoreLayer.Entities;
using CoreLayer.Repositories;
using CoreLayer.Services;
using CoreLayer.UnitOfWork;
using DataLayer.Data;
using DataLayer.Repositories;
using DataLayer.UnitOfWork;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Services;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);
var tokenOption = new TokenOption();
// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(x =>
{
	x.RegisterValidatorsFromAssemblyContaining<StartupBase>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IService<,>), typeof(Service<,>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddDbContext<VtContext>(op =>
{
	op.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

builder.Services.AddIdentity<User, IdentityRole>(op =>
{
	op.User.RequireUniqueEmail = true;
	op.Password.RequireNonAlphanumeric = false;
})
	.AddEntityFrameworkStores<VtContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthentication(op =>
{
	op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
	opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
	{
		ValidIssuer = tokenOption.Issuer,
		ValidAudience = tokenOption.Audience[0],
		IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOption.SecurityKey),

		ValidateIssuerSigningKey = true,
		ValidateAudience = true,
		ValidateIssuer = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero
	};
});

var app = builder.Build();

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
