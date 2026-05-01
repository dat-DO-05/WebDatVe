using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class Phim
{
    [Key]
    public int idPhim { get; set; }

    public string tenPhim { get; set; }
    public string daoDien { get; set; }
    public string moTa { get; set; }
    public string dienVien { get; set; }
    public string loaiPhim { get; set; }
    public int thoiLuong { get; set; }
    public string hinhAnh { get; set; }
    public string trailer { get; set; }
}