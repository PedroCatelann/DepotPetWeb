
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames = false).AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer("Data Source=localhost;Initial Catalog=depotpetweb_db;Integrated Security=True;TrustServerCertificate=true"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/User/Login"; // Página de login
    options.AccessDeniedPath = ""; // Página de acesso negado
});

builder.Services.AddHealthChecks();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();
app.UseAuthentication();;
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

   endpoints.MapControllers();

});

app.UseCookiePolicy(cookiePolicyOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}");

app.Run();
