using AuthenticationAuthorizationpractice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<AppDbContext>(db => db.UseSqlServer(builder.Configuration.GetConnectionString("ConEmp")));
builder.Services.AddScoped<IEmployeetRepository, EmployeeRepository>();



builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(5));





builder.Services.AddAuthentication()
.AddGoogle(options =>
{
    options.ClientId = "943319066767-2i8qdn089rc374l19037aaq1dqba8184.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.SignInScheme = "Identity.External";

});
//.AddFacebook(options =>
// {
//     options.ClientId = "2061380120909574";
//     options.ClientSecret = "f04c14e207b56a05797bad9ce41178d9";
//     options.SignInScheme = "Identity.External";

// });


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EditEmployeePolicy",
        policy => policy.RequireClaim("Edit Role"));
});



builder.Services.AddControllersWithViews();

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
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
