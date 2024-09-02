using UserManagement.DAL;
using UserManagement.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register session services
builder.Services.AddSession(); // This registers the necessary services for session state
builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor for accessing HttpContext in other services

// Register DalRegister as the implementation for IDALRgister
builder.Services.AddScoped<IDALRgister, DalRegister>();
builder.Services.AddScoped<IDALLogin, DALLogin>();
builder.Services.AddScoped<IDALDashbordcs, DALDashbord>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session before the authorization middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Register}/{action=Index}/{id?}");

app.Run();
