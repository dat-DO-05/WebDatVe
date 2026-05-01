using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class NguoiDung
{
    [Key]
    public int idNguoiDung { get; set; }

    public string tenDangNhap { get; set; }
    public string matKhau { get; set; }
    public string email { get; set; }
    public string ten { get; set; }
    public string sdt { get; set; }
    public string vaiTro { get; set; }
    public int diemTichLuy { get; set; }
}