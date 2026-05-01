using System.ComponentModel.DataAnnotations;

namespace WebDatVe.Models;

public class DichVu
{
    [Key]
    public int idDichVu { get; set; }

    public string tenDichVu { get; set; }
    public string loaiDichVu { get; set; }
    public decimal giaTien { get; set; }
    public string moTa { get; set; }
    public string hinhAnh { get; set; }
    public string trangThai { get; set; }
}