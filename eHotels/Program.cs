using Data;
using Microsoft.AspNetCore.Http;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//Dependency inject the database
builder.Services.AddSingleton<DBContext>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db=scope.ServiceProvider.GetRequiredService<DBContext>();
    await db.SeedDatabase();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
