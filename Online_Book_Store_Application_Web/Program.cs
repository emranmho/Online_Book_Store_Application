using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.Repository.Repository;
using Online_Book_Store_Application.Repository.Repository.IRepository;
using Online_Book_Store_Application.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BookDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DataConnectionName")
    ));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
