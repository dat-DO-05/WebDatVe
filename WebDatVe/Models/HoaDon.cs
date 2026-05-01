using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class HoaDon
{
    [Key]
    public int idHoaDon { get; set; }

    public int idNguoiDung { get; set; }
    public DateTime ngayTao { get; set; }
    public decimal tongTien { get; set; }
    public required string trangThai { get; set; }

    public int? idKhuyenMai { get; set; }
}