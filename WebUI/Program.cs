using Application.Common.InterFaces.Messager;
using Application.Common.Messager.Entities;
using WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// اضافه کردن تنظیمات به IConfiguration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// نگاشت تنظیمات به کلاس RabbitMqConfiguration
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));
builder.Services.AddScoped<IClientMessager, RabbitMQClientMessager>();
builder.Services.AddScoped<IServerMessager, RabbitMQServerMessager>();

//builder.Services.AddHostedService<RabbitMQHandler>();


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
