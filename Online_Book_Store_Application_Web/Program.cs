using Microsoft.EntityFrameworkCore;
using Online_Book_Store_Application.Repository.Repository;
using Online_Book_Store_Application.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Online_Book_Store_Application.Utility;
using Stripe;
using Online_Book_Store_Application.DataAccess.Data;
using Online_Book_Store_Application.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BookDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DataConnectionName")
    ));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<BookDbContext>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "724520195892829";
    option.AppSecret = "67fd6a8028e08350ac329bc6048057ac";
});
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.LogoutPath = $"/Identity/Account/Logout";
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IOTimeout = TimeSpan.FromMinutes(100);
    option.Cookie.HttpOnly= true ;
    option.Cookie.IsEssential = true ;
});

//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add(new CustomMiddleWare());
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("Server");
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.Remove("X-AspNet-Version");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

//app.UseMiddleware<CustomMiddleWare>();
SeedDatabase();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Initalize();
}