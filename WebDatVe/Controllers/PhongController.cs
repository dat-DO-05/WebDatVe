using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class PhongController : ControllerBase
{
    private readonly AppDbContext _context;

    public PhongController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _context.Phong.ToListAsync();
        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var phong = await _context.Phong.FindAsync(id);

        if (phong == null)
            return NotFound("Không tìm thấy phòng");

        return Ok(phong);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Phong model)
    {
        if (model == null)
            return BadRequest("Dữ liệu không hợp lệ");

        _context.Phong.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.idPhong }, model);
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Phong model)
    {
        if (model == null)
            return BadRequest();

        var phong = await _context.Phong.FindAsync(id);

        if (phong == null)
            return NotFound("Không tìm thấy phòng");

        _context.Entry(phong).CurrentValues.SetValues(model);
        await _context.SaveChangesAsync();

        return Ok(phong);
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var phong = await _context.Phong.FindAsync(id);

        if (phong == null)
            return NotFound("Không tìm thấy phòng");

        _context.Phong.Remove(phong);
        await _context.SaveChangesAsync();

        return Ok("Xóa thành công");
    }
}
