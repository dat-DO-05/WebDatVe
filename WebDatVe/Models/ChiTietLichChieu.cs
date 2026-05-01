using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class ChiTietLichChieu
{
    [Key]
    public int idChiTiet { get; set; }

    public int idLichChieu { get; set; }
    public int idChiTietPhong { get; set; }
    public int? idNguoiDung { get; set; }

    public string trangThai { get; set; } = "";
    public DateTime? thoiGianGiu { get; set; }

    // 👉 FIX LỖI 2
    public LichChieu? LichChieu { get; set; }
    public ChiTietPhong? ChiTietPhong { get; set; }
}