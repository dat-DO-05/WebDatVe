using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class ChiTietHoaDonController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChiTietHoaDonController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL (lọc theo hóa đơn)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int idHoaDon)
    {
        var data = await _context.ChiTietHoaDon
            .Where(x => x.idHoaDon == idHoaDon)
            .ToListAsync();

        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ct = await _context.ChiTietHoaDon.FindAsync(id);

        if (ct == null)
            return NotFound("Không tìm thấy chi tiết hóa đơn");

        return Ok(ct);
    }
}
