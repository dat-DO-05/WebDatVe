using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class DichVuController : ControllerBase
{
    private readonly AppDbContext _context;

    public DichVuController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? loai)
    {
        var query = _context.DichVu.AsQueryable();

        if (!string.IsNullOrEmpty(loai))
            query = query.Where(x => x.loaiDichVu == loai);

        var data = await query.ToListAsync();
        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dv = await _context.DichVu.FindAsync(id);

        if (dv == null)
            return NotFound("Không tìm thấy dịch vụ");

        return Ok(dv);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DichVu model)
    {
        if (model == null)
            return BadRequest("Dữ liệu không hợp lệ");

        _context.DichVu.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.idDichVu }, model);
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DichVu model)
    {
        if (model == null)
            return BadRequest();

        var dv = await _context.DichVu.FindAsync(id);

        if (dv == null)
            return NotFound("Không tìm thấy dịch vụ");

        _context.Entry(dv).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();

        return Ok(dv);
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dv = await _context.DichVu.FindAsync(id);

        if (dv == null)
            return NotFound("Không tìm thấy dịch vụ");

        _context.DichVu.Remove(dv);
        await _context.SaveChangesAsync();

        return Ok("Xóa thành công");
    }
}
