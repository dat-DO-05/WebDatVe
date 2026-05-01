using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class PhimController : ControllerBase
{
    private readonly AppDbContext _context;

    public PhimController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _context.Phim.ToListAsync();
        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var phim = await _context.Phim.FindAsync(id);

        if (phim == null)
            return NotFound("Không tìm thấy phim");

        return Ok(phim);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Phim model)
    {
        if (model == null)
            return BadRequest("Dữ liệu không hợp lệ");

        _context.Phim.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.idPhim }, model);
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Phim model)
    {
        if (model == null)
            return BadRequest();

        var phim = await _context.Phim.FindAsync(id);

        if (phim == null)
            return NotFound("Không tìm thấy phim");

        // update nhanh
        _context.Entry(phim).CurrentValues.SetValues(model);

        await _context.SaveChangesAsync();

        return Ok(phim);
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var phim = await _context.Phim.FindAsync(id);

        if (phim == null)
            return NotFound("Không tìm thấy phim");

        _context.Phim.Remove(phim);
        await _context.SaveChangesAsync();

        return Ok("Xóa thành công");
    }
}