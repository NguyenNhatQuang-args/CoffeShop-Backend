using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using WebApp_Coffe.Data;
using WebApp_Coffe.Middleware;
using WebApp_Coffe.Repositories;
using WebApp_Coffe.Services;

// Load .env file (nằm ở solution root, 1 cấp trên project)
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (File.Exists(envPath))
    DotNetEnv.Env.Load(envPath);

var builder = WebApplication.CreateBuilder(args);

// Override config from environment variables (.env → env vars → override appsettings)
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDbContext<CoffeeShopDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null)));

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// CORS
var corsOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "super_secret_key_12345678901234567890");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "CoffeeShopApi",
            ValidAudience = jwtSettings["Audience"] ?? "CoffeeShopApi",
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        // Extract token from HttpOnly cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("jwtToken"))
                {
                    context.Token = context.Request.Cookies["jwtToken"];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseCors("DefaultCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
