using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;
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
var allowedOrigins = builder.Configuration
    .GetSection("CorsSettings:AllowedOrigins")
    .Get<string[]>()
    ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", policy =>
    policy.WithOrigins(allowedOrigins)
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()
));

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

// =====================================================================
// RATE LIMITING
// =====================================================================
builder.Services.AddRateLimiter(options =>
{
    // 1. Login endpoint: chống brute-force
    //    Giới hạn 5 lần thử / 1 phút / mỗi IP
    options.AddFixedWindowLimiter("LoginRateLimit", opt =>
    {
        opt.Window          = TimeSpan.FromMinutes(1);
        opt.PermitLimit     = 5;
        opt.QueueLimit      = 0;   // không xếp hàng, từ chối ngay
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // 2. Global API: chống DDoS cơ bản
    //    Giới hạn 120 request / 1 phút / mỗi IP
    options.AddFixedWindowLimiter("GlobalRateLimit", opt =>
    {
        opt.Window          = TimeSpan.FromMinutes(1);
        opt.PermitLimit     = 120;
        opt.QueueLimit      = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Trả về 429 Too Many Requests với message rõ ràng
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode  = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        var isLogin = context.HttpContext.Request.Path
            .StartsWithSegments("/api/auth/login");

        var message = isLogin
            ? "Quá nhiều lần đăng nhập thất bại. Vui lòng thử lại sau 1 phút."
            : "Quá nhiều yêu cầu. Vui lòng thử lại sau.";

        await context.HttpContext.Response.WriteAsync(
            $"{{\"success\":false,\"message\":\"{message}\"}}",
            cancellationToken);
    };
});

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Auto migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CoffeeShopDbContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("Migration completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
        throw;
    }
}

// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("GlobalRateLimit");

app.Run();
