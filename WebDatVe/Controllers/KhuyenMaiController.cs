using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class KhuyenMaiController : ControllerBase
{
    private readonly AppDbContext _context;

    public KhuyenMaiController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL (chỉ lấy đang hoạt động)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var now = DateTime.Now;

        var data = await _context.KhuyenMai
            .Where(x => x.trangThai      == "HOAT_DONG" &&
                        x.thoiGianBatDau  <= now &&
                        x.thoiGianKetThuc >= now)
            .ToListAsync();

        return Ok(data);
    }

    // GET ALL kể cả hết hạn (dành cho admin)
    [HttpGet("tatca")]
    public async Task<IActionResult> GetAllAdmin()
    {
        var data = await _context.KhuyenMai.ToListAsync();
        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var km = await _context.KhuyenMai.FindAsync(id);

        if (km == null)
            return NotFound("Không tìm thấy khuyến mãi");

        return Ok(km);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KhuyenMai model)
    {
        if (model == null)
            return BadRequest("Dữ liệu không hợp lệ");

        model.trangThai = "HOAT_DONG";

        _context.KhuyenMai.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.idKhuyenMai }, model);
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] KhuyenMai model)
    {
        if (model == null)
            return BadRequest();

        var km = await _context.KhuyenMai.FindAsync(id);

        if (km == null)
            return NotFound("Không tìm thấy khuyến mãi");

        _context.Entry(km).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();

        return Ok(km);
    }

    // =========================
    // DELETE (soft delete)
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var km = await _context.KhuyenMai.FindAsync(id);

        if (km == null)
            return NotFound("Không tìm thấy khuyến mãi");

        km.trangThai = "HUY";
        await _context.SaveChangesAsync();

        return Ok("Hủy khuyến mãi thành công");
    }
}
