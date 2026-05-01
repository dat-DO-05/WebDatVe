using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class LichChieu
{
    [Key]
    public int idLichChieu { get; set; }

    public int idPhim { get; set; }
    public int idPhong { get; set; }

    public DateTime thoiGianBatDau { get; set; }
    public DateTime thoiGianKetThuc { get; set; }

    // 👉 FIX LỖI 1
    public decimal giaCoSo { get; set; }

    public string trangThai { get; set; } = "";

    // Navigation (không bắt buộc nhưng nên có)
    public Phim? Phim { get; set; }
    public Phong? Phong { get; set; }
}