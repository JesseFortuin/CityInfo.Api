//web apps must be hosted, can use a builder
using CityInfo.API;
using CityInfo.Infrastructue;
using CityInfo.Infrastructue.DataAccess;
using CityInfo.Infrastructue.Services;
using CItyInfo.Application;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
//rolling interval is how often to log

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();//configured providers are cleared

//builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()//replaces default json formatters
.AddXmlDataContractSerializerFormatters();
//registers services for supporting controllers
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//these deal with swashbuckle documentation (its implementation)
builder.Services.AddEndpointsApiExplorer();//used to generate OpenAPI specification

builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";//CityInfo.API

    //where file resides, and name found
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    //telling swashbuckle to use this 
    setupAction.IncludeXmlComments(xmlCommentsFullPath);
    //definition name, and scheme
    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,//basic type

        Scheme = "Bearer",

        Description = "Input a valid token to access this API"
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    //is a dictionary
    {
        {
            new OpenApiSecurityScheme
            {
                //reference the scheme we used when security definition
               //reference object
                Reference = new OpenApiReference{

                    Type = ReferenceType.SecurityScheme,
                    //security scheme id matches definition
                    Id = "CityInfoApiBearerAuth"}
            }, new List<string>() }//value used for tokens
    });
});//registers services used for specification generation

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
//this says whenever using interface, use specified instance of that class
//builder.Services.AddTransient<IMailService, LocalMailService>();

#else

builder.Services.AddTransient<IMailService, CloudMailService>();

#endif

builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(DbContextOptions =>
DbContextOptions.UseSqlite(
    builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

//mock repositories can be added the same way replacing it in the argument
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddScoped<ICitiesFacade, CitiesFacade>();

builder.Services.AddScoped<IMailService, LocalMailService>();

//builder.Services.AddScoped<IMailService, CloudMailService>();

builder.Services.AddScoped<IPointsOfInterestFacade, PointsOfInterestFacade>();

builder.Services.AddScoped<IAuthenticationFacade, AuthenticationFacade>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//bearer is default scheme for bearer token authentication middleware
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
        {
            //configure how validate token
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Authentication:Issuer"],//authorityvalue

                ValidAudience = builder.Configuration["Authentication:Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
            };
        }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFromAntwerp", policy =>
    {
        //has set of requirements that is passed when all is met
        policy.RequireAuthenticatedUser();

        policy.RequireClaim("city", "Antwerp");
    });
});

builder.Services.AddApiVersioning(setupAction =>
{
    //assume default when no verion given
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    //setting default
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    //return supported versions
    setupAction.ReportApiVersions = true;
});

var app = builder.Build();//creates object of type web app object

//Configure the HTTP request pipeline.
//.environment to access and work with an enviroment
if (app.Environment.IsDevelopment())
{
    //middleware handle http requests
    app.UseSwagger();//generates a specification(code)

    app.UseSwaggerUI();//makes a ui documentation for specification
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //adds endpoints for controller actions
    endpoints.MapControllers();
});

app.Run();
