using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class ChiTietHoaDon
{
    [Key]
    public int idChiTiet { get; set; }

    public int idHoaDon { get; set; }

    public string loaiSanPham { get; set; } // VE / DICHVU

    public int? idVe { get; set; }
    public int? idDichVu { get; set; }

    public int soLuong { get; set; }
    public decimal donGia { get; set; }
    public decimal thanhTien { get; set; }
}