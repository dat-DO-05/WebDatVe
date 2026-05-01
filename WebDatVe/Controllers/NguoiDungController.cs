using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatVe.Models;

[ApiController]
[Route("api/[controller]")]
public class NguoiDungController : ControllerBase
{
    private readonly AppDbContext _context;

    public NguoiDungController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _context.NguoiDung
            .Select(x => new {
                x.idNguoiDung,
                x.tenDangNhap,
                x.email,
                x.ten,
                x.sdt,
                x.vaiTro,
                x.diemTichLuy
            }).ToListAsync();

        return Ok(data);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var nd = await _context.NguoiDung.FindAsync(id);

        if (nd == null)
            return NotFound("Không tìm thấy người dùng");

        return Ok(new {
            nd.idNguoiDung,
            nd.tenDangNhap,
            nd.email,
            nd.ten,
            nd.sdt,
            nd.vaiTro,
            nd.diemTichLuy
        });
    }

    // =========================
    // ĐĂNG KÝ
    // =========================
    [HttpPost("dangky")]
    public async Task<IActionResult> DangKy([FromBody] NguoiDung model)
    {
        if (model == null)
            return BadRequest("Dữ liệu không hợp lệ");

        bool trung = await _context.NguoiDung
            .AnyAsync(x => x.tenDangNhap == model.tenDangNhap);

        if (trung)
            return BadRequest("Tên đăng nhập đã tồn tại");

        model.vaiTro      = "USER";
        model.diemTichLuy = 0;

        _context.NguoiDung.Add(model);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.idNguoiDung }, model.idNguoiDung);
    }

    // =========================
    // ĐĂNG NHẬP
    // =========================
    [HttpPost("dangnhap")]
    public async Task<IActionResult> DangNhap([FromBody] DangNhapRequest req)
    {
        var nd = await _context.NguoiDung
            .FirstOrDefaultAsync(x => x.tenDangNhap == req.tenDangNhap
                                   && x.matKhau      == req.matKhau);

        if (nd == null)
            return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

        return Ok(new {
            nd.idNguoiDung,
            nd.tenDangNhap,
            nd.vaiTro
        });
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] NguoiDung model)
    {
        if (model == null)
            return BadRequest();

        var nd = await _context.NguoiDung.FindAsync(id);

        if (nd == null)
            return NotFound("Không tìm thấy người dùng");

        // Chỉ cho cập nhật các field thông tin, không cho đổi vai trò hay điểm qua đây
        nd.email = model.email;
        nd.ten   = model.ten;
        nd.sdt   = model.sdt;

        await _context.SaveChangesAsync();

        return Ok(nd);
    }

    // =========================
    // DELETE
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var nd = await _context.NguoiDung.FindAsync(id);

        if (nd == null)
            return NotFound("Không tìm thấy người dùng");

        _context.NguoiDung.Remove(nd);
        await _context.SaveChangesAsync();

        return Ok("Xóa thành công");
    }
}

public class DangNhapRequest
{
    public string tenDangNhap { get; set; }
    public string matKhau { get; set; }
}
