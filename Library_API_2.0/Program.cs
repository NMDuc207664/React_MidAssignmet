using Library_API_2._0.Application.Interface;
using Library_API_2._0.Application.Repositories;
using Library_API_2._0.Domain.Entities;
using Library_API_2._0.Infrastructure;
using Library_API_2._0.Infrastructure.Authentication;
using Library_API_2._0.Infrastructure.Repository;
using Library_API_2._0.Application.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Library_API_2._0.Infrastructure.Persistence.Interceptors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT token vào đây: Bearer {your token}"
    };
    // Cấu hình cho Swagger dùng JWT    
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        },
        []
        }
    };
    options.AddSecurityRequirement(securityRequirement);

});

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).AddInterceptors(new ProtectedEntitiesInterceptor()); ;
});

builder.Services.AddControllers();
builder.Services.AddScoped<IBookRepo, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ProtectedEntitiesInterceptor>();
builder.Services.AddScoped<IAuthorRepo, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<IGenreRepo, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddScoped<IBorrowingRequestRepo, BorrowingRequestRepository>();
builder.Services.AddScoped<IBorrowingRequestService, BorrowingRequestService>();

builder.Services.AddScoped<IBorrowingDetailRepo, BorrowingDetailRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IRecordRepo, RecordRepository>();
builder.Services.AddScoped<IBorrowingRecordService, BorrowingRecordService>();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
var jwtSettings = builder.Configuration.GetSection("JWTSettings");
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
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddSingleton<JwtHandler>();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
