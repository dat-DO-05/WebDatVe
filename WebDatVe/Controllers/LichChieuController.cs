using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class LichChieuController : ControllerBase
{
    private readonly AppDbContext _context;

    public LichChieuController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL (lọc theo phim)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idPhim)
    {
        var query = _context.LichChieu.AsQueryable();

        if (idPhim.HasValue)
            query = query.Where(x => x.idPhim == idPhim.Value);

        var data = await query
            .OrderBy(x => x.thoiGianBatDau)
            .ToListAsync();

        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lc = await _context.LichChieu.FindAsync(id);

        if (lc == null)
            return NotFound("Không tìm thấy lịch chiếu");

        return Ok(lc);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LichChieu model)
    {
        if (model == null)
            return BadRequest("Dữ liệu không hợp lệ");

        // Kiểm tra trùng lịch phòng
        bool trungLich = await _context.LichChieu.AnyAsync(x =>
            x.idPhong == model.idPhong &&
            x.trangThai == "HOAT_DONG" &&
            x.thoiGianBatDau  < model.thoiGianKetThuc &&
            x.thoiGianKetThuc > model.thoiGianBatDau);

        if (trungLich)
            return BadRequest("Phòng đã có lịch chiếu trong khung giờ này");

        model.trangThai = "HOAT_DONG";
        _context.LichChieu.Add(model);
        await _context.SaveChangesAsync();

        // Tự động tạo ChiTietLichChieu cho tất cả ghế trong phòng
        var danhSachGhe = await _context.ChiTietPhong
            .Where(g => g.idPhong == model.idPhong)
            .ToListAsync();

        var chiTietList = danhSachGhe.Select(g => new ChiTietLichChieu
        {
            idLichChieu    = model.idLichChieu,
            idChiTietPhong = g.idChiTietPhong,
            trangThai      = "TRONG"
        });

        _context.ChiTietLichChieu.AddRange(chiTietList);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.idLichChieu }, model);
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LichChieu model)
    {
        if (model == null)
            return BadRequest();

        var lc = await _context.LichChieu.FindAsync(id);

        if (lc == null)
            return NotFound("Không tìm thấy lịch chiếu");

        _context.Entry(lc).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();

        return Ok(lc);
    }

    // =========================
    // DELETE (soft delete)
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lc = await _context.LichChieu.FindAsync(id);

        if (lc == null)
            return NotFound("Không tìm thấy lịch chiếu");

        lc.trangThai = "HUY";
        await _context.SaveChangesAsync();

        return Ok("Hủy lịch chiếu thành công");
    }
}
