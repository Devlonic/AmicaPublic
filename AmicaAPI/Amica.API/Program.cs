using Amica.API.Data;
using Amica.API.Data.Models;
using Amica.API.Data.Repositories;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data;
using Amica.API.WebServer.Data.Identity.Extentions;
using Amica.API.WebServer.Data.Repositories.Comments;
using Amica.API.WebServer.Data.Repositories.Followers;
using Amica.API.WebServer.Data.Repositories.Likes;
using Amica.API.WebServer.Data.Repositories.Profiles;
using Amica.API.WebServer.Data.Services;
using Amica.API.WebServer.Data.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSwag.Generation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Schema;

var builder = WebApplication.CreateBuilder(args);

// setup settings
// IMPROVE replace with Options pattern
builder.Services.AddSingleton(sp => {
    return Options.Create(new NoSqlDatabaseSettings(builder.Configuration.GetSection("NoSqlDatabase"), new List<Type>() {
        typeof(PostLike),
        typeof(PostComment),
    }));
});
builder.Services.Configure<ReCaptchaOptions>(builder.Configuration.GetSection("reCAPTCHA"), b => {
    Console.WriteLine();
});

// setup logging
builder.Services.AddHttpLogging(l => {
    l.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    l.RequestHeaders.Add("Authorization");
    l.RequestBodyLogLimit = 4096;
    l.ResponseBodyLogLimit = 4096;
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// setup SQL database for AmicaDbContext
builder.Services.AddDbContext<AmicaDbContext>(o => {
    o.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlServer"));
});

// setup caching

// singleton
builder.Services.AddMemoryCache();

// setup throttling
builder.Services.AddSingleton<AccountThrottleService>();

// setup db transactions manager
builder.Services.AddScoped<AmicaSqlTransactionManager>();

//// setup redis
//builder.Services.AddSingleton<AmicaRedisContext>();
//builder.Services.AddSingleton<IConnectionMultiplexer>(o =>
//    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("NoSqlDefault")));


// setup settings managers
builder.Services.AddSingleton<JwtConfig>();

builder.Services.AddSingleton<JsonConfig<Post>>();
builder.Services.AddSingleton<JsonConfig<Profile>>();
builder.Services.AddSingleton<JsonConfig<Account>>();
builder.Services.AddSingleton<JsonConfig<Image>>();
builder.Services.AddSingleton<JsonConfig<PostComment>>();
builder.Services.AddSingleton<JsonConfig<PostLike>>();

builder.Services.AddScoped<JwtManager>();

// nosql repositories
builder.Services.AddSingleton<ILikesRepository, MongoLikesRepository>();
builder.Services.AddSingleton<ICommentsRepository, MongoCommentRepository>();

// sql repositories
builder.Services.AddScoped<IFollowersRepository, FollowersRepository>();
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<IProfilesRepository, ProfilesRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IImagesRepository, LocalhostImagesRepository>();

// captcha services
builder.Services.AddScoped<ICaptchaValidatorService, ReCaptchaValidatorService>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(o => {
    o.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme {
            In = ParameterLocation.Header,
            Description = @"Bearer (paste here your token (remove all brackets) )",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
    o.SwaggerDoc("v1", new OpenApiInfo() {
        Title = "Amica API - v1",
        Version = "v1"
    });
    var filepath = Path.Combine(AppContext.BaseDirectory, "AmicaAPI.xml");
    o.IncludeXmlComments(filepath);
});
//builder.Services.AddSwaggerDocument();
//builder.Services.AddSwaggerGen();
//NSwag.Generation.IOpenApiDocumentGenerator
//builder.Services.AddOpenApiDocument();

builder.Services.AddAmicaIdentity(o => {
    o.Password.RequiredLength = 8;
    o.Password.RequireDigit = true;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = true;
}).AddEntityFrameworkStores<AmicaDbContext>();

var c = builder.Configuration;

var Jwt = (string field) => {
    return c[$"Jwt:{field}"] ?? throw new JsonException($"JWT Setting missing: {field}");
};

builder.Services.AddAuthentication(o => {
    o.DefaultAuthenticateScheme = "Bearer";
    o.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(o => {
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = Jwt("Audience"),
        ValidIssuer = Jwt("Issuer"),
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt("SecurityKey"))),
    };
});

var app = builder.Build();

if ( app.Environment.IsDevelopment() ) {
    app.UseSwagger();
    //app.UseOpenApi();
    //app.UseSwaggerUi3();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
