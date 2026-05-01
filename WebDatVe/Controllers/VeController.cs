using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class VeController : ControllerBase
{
    private readonly AppDbContext _context;

    public VeController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL (lọc theo lịch chiếu)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idLichChieu)
    {
        var query = _context.Ve.AsQueryable();

        if (idLichChieu.HasValue)
            query = query.Where(x => x.idLichChieu == idLichChieu.Value);

        var data = await query.ToListAsync();
        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ve = await _context.Ve.FindAsync(id);

        if (ve == null)
            return NotFound("Không tìm thấy vé");

        return Ok(ve);
    }
}
