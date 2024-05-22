using Microsoft.EntityFrameworkCore;
using NguyenCoffeeWeb;
using NguyenCoffeeWeb.Hubs;
using NguyenCoffeeWeb.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(otp => otp.IdleTimeout = TimeSpan.FromMinutes(5));
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<postgresContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("MyDb")
));
builder.Services.AddSignalR();
WebApplication app = builder.Build();

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

app.UseAuthorization();

app.UseSession(); //session middleware
app.MapHub<SignalrServer>("signalrServer");
app.MapHub<ChatHub>("/chatHub");
app.MapRazorPages();

app.Run();
