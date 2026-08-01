using Blazored.LocalStorage;
using frontend.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

// HttpClient
builder.Services.AddHttpClient("BackEndIdentityDBService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7196/api/");
});

builder.Services.AddHttpClient("BackEndUserDBService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7290/api/");
});

builder.Services.AddHttpClient("BackEndRoomDBService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7138/api/");
});

builder.Services.AddHttpClient("BackEndServiceDBService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7219/api/");
});

// Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<RoomTypeService>();
builder.Services.AddScoped<ServiceService>();
builder.Services.AddScoped<ServiceCategoryService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// app.UseAuthentication();
// app.UseAuthorization();

app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();