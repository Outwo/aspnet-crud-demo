using CrudDemo.Data;
using CrudDemo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan DbContext pakai In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // session 60 menit
    options.Cookie.HttpOnly = true; // aman dari script
    options.Cookie.IsEssential = true; // selalu dikirim
});


var app = builder.Build();

// Tambah data awal (opsional)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Students.Add(new Student { Nama = "Bisma", Email = "bisma@email.com", TanggalLahir = DateTime.Parse("2000-01-01") });
    context.Students.Add(new Student { Nama = "Raynaldi", Email = "raynaldi@email.com", TanggalLahir = DateTime.Parse("2001-02-02") });
    context.SaveChanges();
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Default route langsung ke Students
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();

// Notifikasi otomatis saat session timeout
app.Use(async (context, next) =>
{
    if (string.IsNullOrEmpty(context.Session.GetString("username")) 
        && context.Request.Path.StartsWithSegments("/Students"))
    {
        context.Response.Redirect("/Account/Login?message=timeout");
        return;
    }
    await next();
});
