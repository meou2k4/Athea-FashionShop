using FashionShop.Data.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình CORS cho phép Frontend gọi API
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FashionShop API", Version = "v1" });

    // Cấu hình Nút Authorize (chỉ cần paste token, Swagger tự thêm "Bearer ")
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Nhập JWT token vào ô bên dưới (KHÔNG cần gõ 'Bearer ', hệ thống tự thêm).",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Cấu hình Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Repository và Service
builder.Services.AddScoped(typeof(FashionShop.Data.Repositories.IRepository<>), typeof(FashionShop.Data.Repositories.Repository<>));
builder.Services.AddScoped<FashionShop.Data.Repositories.IProductRepository, FashionShop.Data.Repositories.ProductRepository>();

builder.Services.AddScoped<FashionShop.Api.Services.IAuthService, FashionShop.Api.Services.AuthService>();
builder.Services.AddScoped<FashionShop.Api.Services.ICategoryService, FashionShop.Api.Services.CategoryService>();
builder.Services.AddScoped<FashionShop.Api.Services.IProductService, FashionShop.Api.Services.ProductService>();
builder.Services.AddScoped<FashionShop.Api.Services.IPropertyService, FashionShop.Api.Services.PropertyService>();
builder.Services.AddScoped<FashionShop.Api.Services.ISettingService, FashionShop.Api.Services.SettingService>();
builder.Services.AddScoped<FashionShop.Api.Services.IEmailService, FashionShop.Api.Services.EmailService>();

// Cấu hình Email
builder.Services.Configure<FashionShop.Api.Models.EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

// Cấu hình JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FashionShop API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root (http://localhost:<port>/)
    });
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseStaticFiles(); // Serve ảnh từ wwwroot/

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    FashionShop.Api.Data.SeedData.Initialize(services);
}

app.Run();
