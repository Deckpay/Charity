using System.Security.Principal;
using Charity.Web.Components;
using Charity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Charity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Charity.Infrastructure.Data.Seed;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<CharityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<CharityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<DbInitializer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeAsync();
}

app.Run();

// Install-Package Microsoft.AspNetCore.Identity.UI -Version 8.0.10 -Project Charity.Web
// A Blazor Server app megoldja automatikusan az Identity függõségeket, ha így telepíted:
// Ez több komponenssel együtt jön:
// Identity
// Identity razor pages
// Login / Register scaffold lehetõség