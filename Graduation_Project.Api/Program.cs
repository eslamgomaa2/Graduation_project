using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interfaces;
using Repository.Implementation;
using OA.Service.Contract;
using OA.Service.Implementation;
using OA.Domain.Auth;
using OA.Domain.Settings;
using Domins.Model;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Domins.Helper;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddDbContext<ApplicationDbcontext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbcontext>()
    .AddDefaultTokenProviders();
builder.Services.AddMemoryCache();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    
}); 
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IBaseRepository<Doctor>,BaseRepositry<Doctor>>();
builder.Services.AddTransient<IBaseRepository<Patient>,BaseRepositry<Patient>>();
builder.Services.AddTransient<IAccountServices,AccountServices>();
builder.Services.AddTransient<IAlarmServices, AlarmServices>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IDoctorService, DoctorService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IWarningService, WarningService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<FirebaseSensorService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    string firebaseUrl = configuration["FirebaseConfig:FirebaseUrl"];
    string authSecret = configuration["FirebaseConfig:AuthSecret"];
    return new FirebaseSensorService(firebaseUrl, authSecret);
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    }).

    AddGoogle(options =>
    {

        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });
    


//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Elderly Care System ", Version = "v1" });
    options.DescribeAllParametersInCamelCase();
    options.InferSecuritySchemes();
});
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                new string[]{}
                    }
                });
});


var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new NullFileProvider()
});



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
