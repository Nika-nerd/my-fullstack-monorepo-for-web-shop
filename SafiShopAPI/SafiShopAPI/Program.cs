using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using SafiShopAPI.Data;
using SafiShopAPI.Models;
using SafiShopAPI.Services;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SafiShop API",
        Version = "v1",
        Description = "API for clothing shop",
    });

    c.TagActionsBy(api => new[]
    {
        api.GroupName ?? api.ActionDescriptor.RouteValues["controller"]
    });

    c.DocInclusionPredicate((name, api) => true);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


var app = builder.Build();



    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafiShop API V1");
        c.RoutePrefix = string.Empty;
    });



app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();



app.Run();

