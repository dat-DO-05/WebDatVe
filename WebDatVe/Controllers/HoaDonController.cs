using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class HoaDonController : ControllerBase
{
    private readonly AppDbContext _context;

    public HoaDonController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL (lọc theo người dùng)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idNguoiDung)
    {
        var query = _context.HoaDon.AsQueryable();

        if (idNguoiDung.HasValue)
            query = query.Where(x => x.idNguoiDung == idNguoiDung.Value);

        var data = await query
            .OrderByDescending(x => x.ngayTao)
            .ToListAsync();

        return Ok(data);
    }

    // =========================
    // GET BY ID (kèm chi tiết)
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var hd = await _context.HoaDon.FindAsync(id);

        if (hd == null)
            return NotFound("Không tìm thấy hóa đơn");

        var chiTiet = await _context.ChiTietHoaDon
            .Where(x => x.idHoaDon == id)
            .ToListAsync();

        return Ok(new { hoaDon = hd, chiTiet });
    }

    // =========================
    // TẠO HÓA ĐƠN (đặt vé + dịch vụ)
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaoHoaDonRequest req)
    {
        if (req == null)
            return BadRequest("Dữ liệu không hợp lệ");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            decimal tongTien = 0;
            var chiTietList = new List<ChiTietHoaDon>();

            // --- Xử lý vé ---
            foreach (var item in req.danhSachVe)
            {
                var ctlc = await _context.ChiTietLichChieu
                    .Include(x => x.LichChieu)
                    .Include(x => x.ChiTietPhong)
                    .FirstOrDefaultAsync(x =>
                        x.idLichChieu    == item.idLichChieu &&
                        x.idChiTietPhong == item.idChiTietPhong &&
                        x.idNguoiDung    == req.idNguoiDung &&
                        x.trangThai      == "DANG_DAT");

                if (ctlc == null)
                    return BadRequest($"Ghế {item.idChiTietPhong} không hợp lệ hoặc đã hết thời gian giữ");

                decimal giaTien = ctlc.LichChieu.giaCoSo * ctlc.ChiTietPhong.heSoGia;

                var ve = new Ve
                {
                    idLichChieu    = item.idLichChieu,
                    idChiTietPhong = item.idChiTietPhong,
                    giaTien        = giaTien
                };
                _context.Ve.Add(ve);
                await _context.SaveChangesAsync();

                // Đánh dấu ghế đã đặt
                ctlc.trangThai = "DA_DAT";

                chiTietList.Add(new ChiTietHoaDon
                {
                    loaiSanPham = "VE",
                    idVe        = ve.idVe,
                    soLuong     = 1,
                    donGia      = giaTien,
                    thanhTien   = giaTien
                });
                tongTien += giaTien;
            }

            // --- Xử lý dịch vụ ---
            foreach (var item in req.danhSachDichVu)
            {
                var dv = await _context.DichVu.FindAsync(item.idDichVu);

                if (dv == null)
                    return BadRequest($"Dịch vụ {item.idDichVu} không tồn tại");

                decimal thanhTien = dv.giaTien * item.soLuong;

                chiTietList.Add(new ChiTietHoaDon
                {
                    loaiSanPham = "DICHVU",
                    idDichVu    = item.idDichVu,
                    soLuong     = item.soLuong,
                    donGia      = dv.giaTien,
                    thanhTien   = thanhTien
                });
                tongTien += thanhTien;
            }

            // --- Áp dụng khuyến mãi ---
            if (req.idKhuyenMai.HasValue)
            {
                var km = await _context.KhuyenMai.FindAsync(req.idKhuyenMai.Value);
                if (km != null && km.trangThai == "HOAT_DONG")
                {
                    tongTien = km.loaiGiamGia == "PHANTRAM"
                        ? tongTien * (1 - km.giaTriGiam / 100)
                        : tongTien - km.giaTriGiam;

                    if (tongTien < 0) tongTien = 0;
                }
            }

            // --- Tạo hóa đơn ---
            var hoaDon = new HoaDon
            {
                idNguoiDung = req.idNguoiDung,
                tongTien    = tongTien,
                trangThai   = "THANH_TOAN_CHO",
                idKhuyenMai = req.idKhuyenMai
            };
            _context.HoaDon.Add(hoaDon);
            await _context.SaveChangesAsync();

            // Gắn idHoaDon vào chi tiết
            foreach (var ct in chiTietList)
                ct.idHoaDon = hoaDon.idHoaDon;

            _context.ChiTietHoaDon.AddRange(chiTietList);

            // Cộng điểm tích lũy (100.000đ = 1 điểm)
            var nd = await _context.NguoiDung.FindAsync(req.idNguoiDung);
            if (nd != null)
                nd.diemTichLuy += (int)(tongTien / 100000);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetById), new { id = hoaDon.idHoaDon }, new {
                hoaDon.idHoaDon,
                hoaDon.tongTien,
                hoaDon.trangThai
            });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // =========================
    // CẬP NHẬT TRẠNG THÁI
    // =========================
    [HttpPut("{id}/trangthai")]
    public async Task<IActionResult> CapNhatTrangThai(int id, [FromBody] string trangThai)
    {
        var hd = await _context.HoaDon.FindAsync(id);

        if (hd == null)
            return NotFound("Không tìm thấy hóa đơn");

        hd.trangThai = trangThai;
        await _context.SaveChangesAsync();

        return Ok(hd);
    }
}

// ---- Request classes ----
public class TaoHoaDonRequest
{
    public int        idNguoiDung    { get; set; }
    public int?       idKhuyenMai    { get; set; }
    public List<DatVeItem>    danhSachVe     { get; set; } = new();
    public List<DatDichVuItem> danhSachDichVu { get; set; } = new();
}

public class DatVeItem
{
    public int idLichChieu    { get; set; }
    public int idChiTietPhong { get; set; }
}

public class DatDichVuItem
{
    public int idDichVu { get; set; }
    public int soLuong  { get; set; }
}
