using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class ChiTietPhongController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChiTietPhongController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL (lọc theo phòng)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idPhong)
    {
        var query = _context.ChiTietPhong.AsQueryable();

        if (idPhong.HasValue)
            query = query.Where(x => x.idPhong == idPhong.Value);

        var data = await query.OrderBy(x => x.soGhe).ToListAsync();
        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ghe = await _context.ChiTietPhong.FindAsync(id);

        if (ghe == null)
            return NotFound("Không tìm thấy ghế");

        return Ok(ghe);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ChiTietPhong model)
    {
        if (model == null)
            return BadRequest("Dữ liệu không hợp lệ");

        _context.ChiTietPhong.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.idChiTietPhong }, model);
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ChiTietPhong model)
    {
        if (model == null)
            return BadRequest();

        var ghe = await _context.ChiTietPhong.FindAsync(id);

        if (ghe == null)
            return NotFound("Không tìm thấy ghế");

        _context.Entry(ghe).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();

        return Ok(ghe);
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ghe = await _context.ChiTietPhong.FindAsync(id);

        if (ghe == null)
            return NotFound("Không tìm thấy ghế");

        _context.ChiTietPhong.Remove(ghe);
        await _context.SaveChangesAsync();

        return Ok("Xóa thành công");
    }
}
