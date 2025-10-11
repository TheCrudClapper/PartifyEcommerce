using CSOS.Core;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Infrastructure;
using CSOS.Infrastructure.Repositories;
using CSOS.UI.Filters;
using CSOS.UI.Helpers;
using CSOS.UI.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
//Third Party
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Logging Configuration
// -----------------------------
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestProperties
                            | HttpLoggingFields.ResponsePropertiesAndHeaders;
});

// -----------------------------
// Infrastructure Layer
// -----------------------------
builder.Services.AddInfrastructureLayer(builder.Configuration);

// -----------------------------
// Core Layer
// -----------------------------
builder.Services.AddCoreLayer();

// -----------------------------
// Helpers / Filters
// -----------------------------
builder.Services.AddScoped<ValidatePicturesFilter>();
builder.Services.AddScoped<OfferViewModelInitializer>();

// -----------------------------
// Redis Caching
// -----------------------------
builder.Services.AddStackExchangeRedisCache(options =>
{
    var host = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
    options.Configuration = $"{host}:{port}";
});

// -----------------------------
// Unit of Work
// -----------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// -----------------------------
// Authorization Policies
// -----------------------------
builder.Services.AddAuthorization(options =>
{
    //enforces authorization policy (user must be authenticated)
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build();


    options.AddPolicy("NotAuthorized", policy =>
    {
        //when user is already logged in he cant access given method
        policy.RequireAssertion(context =>
        {
            return !context.User.Identity.IsAuthenticated;
        });
    });
});

// -----------------------------
// Identity / Cookie Settingss
// -----------------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
});

// -----------------------------
//  Anti-forgery
// -----------------------------

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
});
builder.Services.AddHttpContextAccessor();

// -----------------------------
//  Controllers & Views 
// -----------------------------
builder.Services.AddControllersWithViews(options =>
{
    //for every form that uses POST,DELETE,PUT generate csrf token
    //this auto-adds [ValidateAntiForgeryToken] in certain controller actions
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

var app = builder.Build();

// -----------------------------
// Logging & Error Handling
// -----------------------------
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}


// -----------------------------
// Security & Static Files
// -----------------------------
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();


// -----------------------------
// Request Logging
// -----------------------------
app.UseHttpLogging();


// -----------------------------
// Routing & Authentication
// -----------------------------
app.UseRouting();
app.UseAuthentication(); //reads auth cookie and can extract data from it
app.UseAuthorization(); //validates access permissions of the user

// -----------------------------
// Controller Routes
// -----------------------------
app.MapControllerRoute(
    name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// -----------------------------
// Run Application
// -----------------------------
app.Run();