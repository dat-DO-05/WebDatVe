using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Thêm dòng này để dùng ForeignKey

namespace WebDatVe.Models;

public class LichChieu
{
    [Key]
    public int idLichChieu { get; set; }

    // Chỉ định rõ idPhim là khóa ngoại cho thuộc tính Phim
    [ForeignKey("Phim")]
    public int idPhim { get; set; }

    // Chỉ định rõ idPhong là khóa ngoại cho thuộc tính Phong
    [ForeignKey("Phong")]
    public int idPhong { get; set; }

    public DateTime thoiGianBatDau { get; set; }
    public DateTime thoiGianKetThuc { get; set; }

    public decimal giaCoSo { get; set; }

    public string trangThai { get; set; } = "";

    // Dùng dấu ? để tránh lỗi bắt buộc nhập cả cụm object khi POST
    public virtual Phim? Phim { get; set; }
    public virtual Phong? Phong { get; set; }
}