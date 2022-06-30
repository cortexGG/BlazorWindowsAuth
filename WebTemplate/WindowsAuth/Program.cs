using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WindowsAuth.Data;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Authorization;
using WindowsAuth.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);

var devMode = false;
if (devMode)
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Policies.IsAdministrator, policy => policy.Requirements.Add(new Requirements.IsDeveloperRequirement()));
        options.AddPolicy(Policies.IsReportViewer, policy => policy.Requirements.Add(new Requirements.IsDeveloperRequirement()));
        //Note also that there is no fallback policy in dev mode so that we don't need access to AD/the correct groups for testing
    });
                
    builder.Services.AddSingleton<IAuthorizationHandler, Handlers.IsDeveloperHandler>();
}
else
{
    builder.Services.AddAuthorization(options =>
    {
        //Prevents access to any part of the application without the specified role
        //https://andrewlock.net/setting-global-authorization-policies-using-the-defaultpolicy-and-the-fallbackpolicy-in-aspnet-core-3/
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireRole("Everyone")
            .Build();

        options.AddPolicy(Policies.IsAdministrator, policy => policy.RequireRole("Everyone"));
        options.AddPolicy(Policies.IsReportViewer, policy => policy.RequireRole("AD Group Name"));
    });
}

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<WeatherForecastService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();