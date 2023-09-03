using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Hubs;
using prjCoreWebWantWant.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); //為了讓cshtml檔案可以加入Session
builder.Services.AddSignalR();//即時通訊用

builder.Services.AddDbContext<NewIspanProjectContext>(
options => options.UseSqlServer(
builder.Configuration.GetConnectionString("NewIspanProjectConnection")
));

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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.UseWebSockets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//pattern: "{controller=Member}/{action=MemberAccount}/{id?}");

app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<CExpertTask>("/cExpertTaskHub");

app.Run();
