using Microsoft.EntityFrameworkCore;
using QuanLyChiTieu_3TL.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ✅ Thêm CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Cho phép mọi domain (nếu dùng Flutter Web)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Kết nối DB
builder.Services.AddDbContext<QuanLyChiTieu3tlContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnQL3TL")));

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

// ✅ Áp dụng CORS — phải đặt trước UseAuthorization
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
