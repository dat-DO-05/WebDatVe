using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class Phong
{
    [Key] // 🔥 BẮT BUỘC PHẢI CÓ
    public int idPhong { get; set; }

    public string tenPhong { get; set; }
    public string loaiPhong { get; set; }
    public int soLuongGhe { get; set; }
}