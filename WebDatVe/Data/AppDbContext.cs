using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebDatVe.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSet (mapping bảng)
    public DbSet<NguoiDung> NguoiDung { get; set; }
    public DbSet<Phim> Phim { get; set; }
    public DbSet<Phong> Phong { get; set; }
    public DbSet<ChiTietPhong> ChiTietPhong { get; set; }
    public DbSet<LichChieu> LichChieu { get; set; }
    public DbSet<ChiTietLichChieu> ChiTietLichChieu { get; set; }
    public DbSet<Ve> Ve { get; set; }
    public DbSet<DichVu> DichVu { get; set; }
    public DbSet<KhuyenMai> KhuyenMai { get; set; }
    public DbSet<HoaDon> HoaDon { get; set; }
    public DbSet<ChiTietHoaDon> ChiTietHoaDon { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UNIQUE: (idPhong, soGhe)
        modelBuilder.Entity<ChiTietPhong>()
            .HasIndex(x => new { x.idPhong, x.soGhe })
            .IsUnique();

        // UNIQUE: (idLichChieu, idChiTietPhong)
        modelBuilder.Entity<ChiTietLichChieu>()
            .HasIndex(x => new { x.idLichChieu, x.idChiTietPhong })
            .IsUnique();

        // UNIQUE: (idLichChieu, idChiTietPhong) trong Ve
        modelBuilder.Entity<Ve>()
            .HasIndex(x => new { x.idLichChieu, x.idChiTietPhong })
            .IsUnique();
    }
}