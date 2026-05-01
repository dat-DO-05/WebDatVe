using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDatVe.Models;

public class Ve
{
    [Key]
    public int idVe { get; set; }

    // Foreign Keys khớp với SQL của bạn
    public int idLichChieu { get; set; }
    public int idChiTietPhong { get; set; }

    // Thuộc tính này để Controller cũ không bị lỗi đỏ
    public int idChiTietLichChieu { get; set; }

    // SQL của bạn là giaTien, nhưng Controller có thể dùng giaVe, ta để cả 2
    public decimal giaTien { get; set; }
    public decimal giaVe { get; set; }

    // Navigation
    public HoaDon? HoaDon { get; set; }
    public ChiTietLichChieu? ChiTietLichChieu { get; set; }
    public LichChieu? LichChieu { get; set; }
}