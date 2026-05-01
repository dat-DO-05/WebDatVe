using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class ChiTietLichChieuController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChiTietLichChieuController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET SƠ ĐỒ GHẾ theo suất chiếu
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetSoDo([FromQuery] int idLichChieu)
    {
        var lc = await _context.LichChieu.FindAsync(idLichChieu);

        if (lc == null)
            return NotFound("Không tìm thấy lịch chiếu");

        var data = await _context.ChiTietLichChieu
            .Where(x => x.idLichChieu == idLichChieu)
            .Join(_context.ChiTietPhong,
                ctlc => ctlc.idChiTietPhong,
                ctp  => ctp.idChiTietPhong,
                (ctlc, ctp) => new {
                    ctlc.idChiTiet,
                    ctp.soGhe,
                    ctp.loaiGhe,
                    ctp.heSoGia,
                    giaTien  = lc.giaCoSo * ctp.heSoGia,
                    ctlc.trangThai
                })
            .OrderBy(x => x.soGhe)
            .ToListAsync();

        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ct = await _context.ChiTietLichChieu.FindAsync(id);

        if (ct == null)
            return NotFound("Không tìm thấy");

        return Ok(ct);
    }

    // =========================
    // GIỮ GHẾ (5 phút)
    // =========================
    [HttpPost("giu")]
    public async Task<IActionResult> GiuGhe([FromBody] GiuGheRequest req)
    {
        var ct = await _context.ChiTietLichChieu
            .FirstOrDefaultAsync(x =>
                x.idLichChieu    == req.idLichChieu &&
                x.idChiTietPhong == req.idChiTietPhong);

        if (ct == null)
            return NotFound("Không tìm thấy ghế");

        // Nếu đang giữ quá 5 phút thì tự động reset
        if (ct.trangThai == "DANG_DAT" &&
            ct.thoiGianGiu.HasValue &&
            ct.thoiGianGiu.Value.AddMinutes(5) < DateTime.Now)
        {
            ct.trangThai   = "TRONG";
            ct.idNguoiDung = null;
        }

        if (ct.trangThai != "TRONG")
            return BadRequest("Ghế đã được đặt hoặc đang được giữ bởi người khác");

        ct.trangThai   = "DANG_DAT";
        ct.idNguoiDung = req.idNguoiDung;
        ct.thoiGianGiu = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok("Ghế đang được giữ trong 5 phút");
    }

    // =========================
    // THẢ GHẾ
    // =========================
    [HttpPost("tha")]
    public async Task<IActionResult> ThaGhe([FromBody] GiuGheRequest req)
    {
        var ct = await _context.ChiTietLichChieu
            .FirstOrDefaultAsync(x =>
                x.idLichChieu    == req.idLichChieu &&
                x.idChiTietPhong == req.idChiTietPhong &&
                x.idNguoiDung    == req.idNguoiDung);

        if (ct == null)
            return NotFound("Không tìm thấy");

        ct.trangThai   = "TRONG";
        ct.idNguoiDung = null;
        ct.thoiGianGiu = null;

        await _context.SaveChangesAsync();

        return Ok("Đã thả ghế");
    }
}

public class GiuGheRequest
{
    public int idLichChieu    { get; set; }
    public int idChiTietPhong { get; set; }
    public int idNguoiDung    { get; set; }
}
