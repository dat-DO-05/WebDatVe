using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class ChiTietPhong
{
    [Key]
    public int idChiTietPhong { get; set; }

    public int idPhong { get; set; }
    public string soGhe { get; set; }
    public string loaiGhe { get; set; }
    public decimal heSoGia { get; set; } = 1;  // thêm dòng này
}