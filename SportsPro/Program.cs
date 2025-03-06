using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SportsProContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SportsPro")));

builder.Services.AddSession();

//add services above
var app = builder.Build();
//config services below 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
