using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChiTieu_3TL.Data;
using System.Globalization;

namespace QuanLyChiTieu_3TL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiaoDichController : Controller
    {
        private readonly QuanLyChiTieu3tlContext _context;

        public GiaoDichController(QuanLyChiTieu3tlContext context)
        {
            _context = context;
        }

        /// 1. GET: Lấy danh sách giao dịch theo tài khoản, có thể lọc theo tháng/năm 
        // URL: /api/GiaoDich/by-tai-khoan/{idTaiKhoan}?month=MM&year=YYYYi

        [HttpGet("by-tai-khoan/{idTaiKhoan}")]
        public async Task<IActionResult> GetMonthlyReport(int idTaiKhoan, [FromQuery] int? month, [FromQuery] int? year)
        {
            var today = DateTime.Today;
            int m = month ?? today.Month;
            int y = year ?? today.Year;

            // Lấy tất cả giao dịch
            var all = await _context.GiaoDiches
                .Include(g => g.IdDanhMucNavigation)
                .Where(g => g.IdTaiKhoan == idTaiKhoan
                         && g.NgayGiaoDich.Month == m
                         && g.NgayGiaoDich.Year == y)
                .ToListAsync();

            if (!all.Any())
            {
                return Ok(new
                {
                    summary = new
                    {
                        message = "Không có giao dịch nào trong tháng này.",
                        totalIncome = 0,
                        totalExpense = 0,
                        balance = 0
                    },
                    transactions = new List<object>()  // Trả về danh sách rỗng
                });
            }

            // Tính summary
            var totalIncome = all
        .Where(g => g.LoaiGiaoDich.ToLower() == "thu" && g.NgayGiaoDich.Month == m && g.NgayGiaoDich.Year == y)
        .Sum(g => g.SoTien);

            var totalExpense = all
        .Where(g => g.LoaiGiaoDich.ToLower() == "chi")
        .Sum(g => g.SoTien);
            var balance = totalIncome - totalExpense;

            // (Option) Group theo ngày
            var grouped = all
                .GroupBy(g => g.NgayGiaoDich)
                .OrderByDescending(g => g.Key)
                .Select(g => new {
                    date = g.Key.ToString("yyyy-MM-dd"),
                    weekday = g.Key.ToString("dddd", new CultureInfo("vi-VN")),
                    items = g.Select(x => new {
                        x.Id,
                        soTien = x.SoTien,
                        loai = x.LoaiGiaoDich,
                        moTa = x.MoTa,
                        icon = x.IdDanhMucNavigation.Icon,
                        color = x.Color,
                        tenDanhMuc = x.IdDanhMucNavigation.TenDanhMuc
                    })
                });

            return Ok(new
            {
                summary = new
                {
                    totalIncome,
                    totalExpense,
                    balance
                },
                transactions = grouped
            });
        }




        //// 2. GET: Lấy chi tiết một giao dịch theo ID
        //// URL: /api/GiaoDich/{id}
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetGiaoDich(int id)
        //{
        //    var giaoDich = await _context.GiaoDiches
        //        .Include(g => g.DanhMuc)
        //        .FirstOrDefaultAsync(g => g.Id == id);

        //    if (giaoDich == null)
        //    {
        //        return NotFound(new { message = "Không tìm thấy giao dịch." });
        //    }

        //    return Ok(new
        //    {
        //        giaoDich.Id,
        //        giaoDich.SoTien,
        //        giaoDich.LoaiGiaoDich,
        //        NgayGiaoDich = giaoDich.NgayGiaoDich.ToString("yyyy-MM-dd"),
        //        giaoDich.MoTa,
        //        IdDanhMuc = giaoDich.IdDanhMuc,
        //        TenDanhMuc = giaoDich.DanhMuc.TenDanhMuc,
        //        IconDanhMuc = giaoDich.DanhMuc.Icon,
        //        IdTaiKhoan = giaoDich.IdTaiKhoan
        //    });
        //}

        //// 3. POST: Tạo giao dịch mới
        //// URL: /api/GiaoDich
        //[HttpPost]
        //public async Task<IActionResult> CreateGiaoDich([FromBody] GiaoDich giaoDich)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Có thể thêm kiểm tra IdDanhMuc và IdTaiKhoan tồn tại ở đây
        //    var danhMucExists = await _context.DanhMucs.AnyAsync(d => d.Id == giaoDich.IdDanhMuc);
        //    var taiKhoanExists = await _context.TaiKhoans.AnyAsync(tk => tk.Id == giaoDich.IdTaiKhoan);

        //    if (!danhMucExists || !taiKhoanExists)
        //    {
        //        return BadRequest(new { message = "IdDanhMuc hoặc IdTaiKhoan không hợp lệ." });
        //    }

        //    _context.GiaoDiches.Add(giaoDich);
        //    await _context.SaveChangesAsync();

        //    // Trả về giao dịch vừa tạo với Id mới
        //    return CreatedAtAction(nameof(GetGiaoDich), new { id = giaoDich.Id }, giaoDich);
        //}

        //// 4. PUT: Cập nhật giao dịch
        //// URL: /api/GiaoDich/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateGiaoDich(int id, [FromBody] GiaoDich giaoDich)
        //{
        //    if (id != giaoDich.Id)
        //    {
        //        return BadRequest(new { message = "ID giao dịch không khớp." });
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var existingGiaoDich = await _context.GiaoDiches.FindAsync(id);
        //    if (existingGiaoDich == null)
        //    {
        //        return NotFound(new { message = "Không tìm thấy giao dịch để cập nhật." });
        //    }

        //    // Có thể thêm kiểm tra IdDanhMuc và IdTaiKhoan tồn tại ở đây
        //    var danhMucExists = await _context.DanhMucs.AnyAsync(d => d.Id == giaoDich.IdDanhMuc);
        //    var taiKhoanExists = await _context.TaiKhoans.AnyAsync(tk => tk.Id == giaoDich.IdTaiKhoan);

        //    if (!danhMucExists || !taiKhoanExists)
        //    {
        //        return BadRequest(new { message = "IdDanhMuc hoặc IdTaiKhoan không hợp lệ." });
        //    }

        //    // Cập nhật các trường dữ liệu
        //    existingGiaoDich.SoTien = giaoDich.SoTien;
        //    existingGiaoDich.LoaiGiaoDich = giaoDich.LoaiGiaoDich;
        //    existingGiaoDich.NgayGiaoDich = giaoDich.NgayGiaoDich;
        //    existingGiaoDich.MoTa = giaoDich.MoTa;
        //    existingGiaoDich.IdDanhMuc = giaoDich.IdDanhMuc;
        //    existingGiaoDich.IdTaiKhoan = giaoDich.IdTaiKhoan;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!await GiaoDichExists(id))
        //        {
        //            return NotFound(new { message = "Giao dịch không tồn tại." });
        //        }
        //        else
        //        {
        //            throw; // Ném lỗi nếu có vấn đề khác
        //        }
        //    }

        //    return NoContent(); // Trả về 204 No Content nếu cập nhật thành công
        //}

        //// 5. DELETE: Xóa giao dịch
        //// URL: /api/GiaoDich/{id}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGiaoDich(int id)
        //{
        //    var giaoDich = await _context.GiaoDiches.FindAsync(id);
        //    if (giaoDich == null)
        //    {
        //        return NotFound(new { message = "Không tìm thấy giao dịch để xóa." });
        //    }

        //    _context.GiaoDiches.Remove(giaoDich);
        //    await _context.SaveChangesAsync();

        //    return NoContent(); // Trả về 204 No Content nếu xóa thành công
        //}

        //private async Task<bool> GiaoDichExists(int id)
        //{
        //    return await _context.GiaoDiches.AnyAsync(e => e.Id == id);
        //}
    }
}

