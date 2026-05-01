using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class KhuyenMai
{
    [Key]
    public int idKhuyenMai { get; set; }

    public string tenKhuyenMai { get; set; }
    public string moTa { get; set; }
    public string loaiGiamGia { get; set; }
    public decimal giaTriGiam { get; set; }
    public string dieuKienApDung { get; set; }

    public DateTime thoiGianBatDau { get; set; }
    public DateTime thoiGianKetThuc { get; set; }

    public string trangThai { get; set; }
}