using AegisLabsExam.Extensions;
using AegisLabsExam.Helpers;
using AegisLabsExam.Repositories;
using NokoCore.PyLike3;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHsts(options =>
{
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(30);
});

// builder.Services.AddSingleton<T>(instance) (One instance for the entire application lifetime)
// builder.Services.AddScoped<T>(instance) (One instance per request, same instance throughout the request lifecycle)
// builder.Services.AddTransient<T>(instance) (New instance every time it is requested)

builder.Services.AddDatabaseHelper(options =>
{
    // Use the plug-in scripts
    options.Scripts = [ Path.Combine(AppContext.BaseDirectory, "Assets", "Scripts", "Person.CTE.sql") ];
});

// builder.Services.AddSingleton<IDatabaseHelper, DatabaseHelper>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
    
builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

// var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

// Trigger the database helper execution scripts
app.UseDatabaseHelperScripts();

app.UsePyLike3(options =>
{
    options.PrintPlatformInfo = true;
    options.InterpreterPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Bin", "python.exe");
});

// Plug in Rotativa
app.UseRotativa(options =>
{
    options.RootPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Bin");
});

app.MapStaticAssets();

app.MapControllers();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}")
//     .WithStaticAssets();
//
// app.MapControllerRoute(
//     name: "pdf", 
//     pattern: "{controller=Pdf}/{action=Index}/{id?}");

app.Run();
