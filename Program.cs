using CrudDemo.Data;
using CrudDemo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan DbContext pakai In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

builder.Services.AddControllersWithViews();

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
app.UseAuthorization();

// Default route langsung ke Students
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();
