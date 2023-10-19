using System.Reflection;
using System.Text;
using FortunaDigital.Application.Services.Implementations;
using FortunaDigital.Application.Services.Interfaces;
using FortunaDigital.Infraistructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MariaDbServerVersion(new Version(10, 4, 12));
var connectionString = builder.Configuration.GetConnectionString("MySql");

builder.Host.UseSerilog(Log.Logger)
    .ConfigureAppConfiguration((hostingContext, config) => {
        var configurationRoot = config.Build();

        Serilog.Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.MySQL(connectionString).CreateLogger();
    }).UseSerilog();

builder.Services.AddDbContext<FortunaDigitalDbContext>(
    options => options.UseMySql(connectionString, serverVersion), optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddScoped<FortunaDigitalDbContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRaffleService, RaffleService>();
builder.Services.AddScoped<IRaffleNumbersService, RaffleNumbersService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fortuna Digital API", Version = "v1" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme {
            Description = @"Cabeçalho de autorização JWT usando o esquema Bearer. \r\n\r\n 
                      Digite 'Bearer' [espaço] e, em seguida, seu token na entrada de texto abaixo.
                      Exemplo: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();
    options.AddPolicy("EmployeePolicy", p =>
        p.RequireAuthenticatedUser().RequireClaim("employee"));
    options.AddPolicy("UserPolicy", p =>
        p.RequireAuthenticatedUser().RequireClaim("user"));
    options.AddPolicy("Integration", p =>
        p.RequireAuthenticatedUser().RequireClaim("integration"));
});

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});

var app = builder.Build();

app.UseRouting();

app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fortuna Digital V1");
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext http) => {
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;

    if (error != null) {
        switch (error) {
            case MySqlException:
                return Results.Problem(title: "Databse out", statusCode: 500);
            case FormatException:
                return Results.Problem(title: "Error to convert data to other type. Confirm all information sent", statusCode: 500);
        }
    }

    return Results.Problem(title: "An error ocurred", statusCode: 500);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

