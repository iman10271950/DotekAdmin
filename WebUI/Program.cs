using WebUI;
using Microsoft.AspNetCore.Hosting;
using Prometheus;
using Infrastructure;
using Application;
using Application.Common.Extentions;
using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Entities;
using WebUI.Services;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Services;
using Application.Common.Methodes;
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // Nothing here is fine if config is in appsettings
});


string envirment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
if (envirment == "Staging")
{
    builder.Configuration.AddJsonFile($"appsettings.json");
}
else
{
    builder.Configuration.AddJsonFile($"appsettings.{envirment}.json");
}

// اضافه کردن تنظیمات به IConfiguration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<OtherServicesAuth_VM>(
    builder.Configuration.GetSection("AuthDefaults"));

// نگاشت تنظیمات به کلاس RabbitMqConfiguration
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));
builder.Services.AddScoped<IClientMessager, RabbitMQClientMessager>();
builder.Services.AddScoped<IServerMessager, RabbitMQServerMessager>();
// Add services to the container.
builder.Services.AddScoped<IAuthHelper,AuthHelper>();
//builder.Services.AddImplementations();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices(builder.Configuration);

builder.Services.Configure<MessageSecuritySettings>(
    builder.Configuration.GetSection("MessageSecurity"));


var app = builder.Build();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseDeveloperExceptionPage();
//}
app.UseCors("AllowAll");
var staging = app.Environment.IsStaging();

if ((envirment ?? "") != "Production")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(0);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.EnableFilter();
        c.EnablePersistAuthorization();
        c.DisplayRequestDuration();
    });
}

//if (!app.Environment.IsDevelopment())
//{
//    app.UseHsts();
//}

//}
//app.UseHttpRedirection();
app.UseRouting();
app.UseHttpMetrics();
//app.UseCors(option => option.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader()
//               .AllowCredentials());
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod()
.SetIsOriginAllowed(origin => true)
.AllowCredentials().WithExposedHeaders("Authorization").WithMethods("GET", "POST", "DELETE", "PUT", "OPTIONS"));

app.UseStaticFiles();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics();
});
//app.UseMiddleware<CustomHeaderMiddleware>();
app.UseStatusCodePages();

app.Run();

public partial class Program { }