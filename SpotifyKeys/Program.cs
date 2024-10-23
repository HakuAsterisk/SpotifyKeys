using SpotifyKeys.Components;
using SpotifyKeys.Components.Services;
//This builds the entire razor program and everything related to it. Above you can see refrences to files I've added
//in order to add the API functionalities we might need.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient, neccesary for fetching information from an API.
builder.Services.AddHttpClient();

// Register Spotify Service, this is where we the methods needed to fetch and handle the API information
builder.Services.AddScoped<SpotifyService>();
// Also register Spotify Authentication, this is where we authenticate ourselves to the SpotifyAPI
builder.Services.AddScoped<ApiVerified>();

//Everything below is standard blazor functionality, do not touch.

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();