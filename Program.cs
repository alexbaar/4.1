using Microsoft.OpenApi.Models;  // for line 12
using System.Reflection; // for line 26 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// enable generation of the OAS file (OpenAPI Spec) 
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Robot Controller API",
        Description = "New backend service that provides resources for the Moon robot simulator.",
    Contact = new OpenApiContact
    {
     Name = "Aleksandra Bartosiak",
     Email = "abartosiak@deakin.edu.au"
    },

    });

    var xmlFilename = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,xmlFilename));

});



// 3.1P:
// AddScoped services are created once per request

// dependency injection -> provides a way to separate the creation of object from its usage; makes testing easier 
// because we can mock the dependencies 
// useful when we need to inject the same dependency into multiple components

// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommand>();
// builder.Services.AddScoped<IMapDataAccess, Map>();




var app = builder.Build();

//instruct the application to use the OpenAPI Spec file and the UI for it
if (app.Environment.IsDevelopment()) // OpenAPI UI and OAS is available only for the development environment
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/theme-newspaper.css"));
}

//enable static file usage : CSS, favicons, images from wwwroot folder
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
