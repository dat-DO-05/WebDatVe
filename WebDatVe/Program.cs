using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. ADD SERVICES
// =========================================================//

// Cấu hình Database (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// 🔥 QUAN TRỌNG: Thêm cấu hình CORS để sửa lỗi "Failed to fetch" ở image_4b2975.png
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================================================
// 2. BUILD APP
// =========================================================
var app = builder.Build();

// =========================================================
// 3. CONFIG PIPELINE (Thứ tự ở đây rất quan trọng)
// =========================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Movie Booking V1");
        c.RoutePrefix = string.Empty; // Mở Swagger ngay tại trang chủ "/"
    });
}

// 🔥 Kích hoạt CORS (Phải đặt TRƯỚC Authorization và MapControllers)
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();