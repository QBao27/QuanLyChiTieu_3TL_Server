using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChiTieu_3TL.Data;
using QuanLyChiTieu_3TL.Models;
using System.Globalization;
using System.Reflection.Metadata;

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
        .Where(g => g.LoaiGiaoDich.ToLower() == "thu")
        .Sum(g => g.SoTien);

            var totalExpense = all
        .Where(g => g.LoaiGiaoDich.ToLower() == "chi")
        .Sum(g => g.SoTien);
            var balance = totalIncome - totalExpense;

            // (Option) Group theo ngày
            var grouped = all
     .GroupBy(g => g.NgayGiaoDich)
     .OrderByDescending(g => g.Key) // sắp xếp các ngày giảm dần
     .Select(g => new {
         date = g.Key.ToString("yyyy-MM-dd"),
         weekday = g.Key.ToString("dddd", new CultureInfo("vi-VN")),
         items = g.OrderByDescending(x => x.Id) // sắp xếp giao dịch trong ngày giảm dần
                   .Select(x => new {
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


        // 3. POST: Tạo giao dịch mới
        // URL: /api/GiaoDich
        [HttpPost("by-tai-khoan/{idTaiKhoan}")]
        public async Task<IActionResult> ThemGiaoDich(int idTaiKhoan, [FromBody] thongTinGiaoDich dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Thông tin không hợp lệ");

                if (dto.SoTien <= 0)
                    return BadRequest("Số tiền phải lớn hơn 0");

                var giaoDich = new GiaoDich
                {
                    IdTaiKhoan = idTaiKhoan,
                    LoaiGiaoDich = dto.LoaiGiaoDich.ToLower(),
                    SoTien = dto.SoTien,
                    NgayGiaoDich = dto.NgayGiaoDich,
                    MoTa = dto.MoTa,
                    IdDanhMuc = dto.IdDanhMuc,
                    Color = dto.Color,
                };

                _context.GiaoDiches.Add(giaoDich);
                await _context.SaveChangesAsync();

                return Ok(giaoDich); // ✅ Trả về 200 OK kèm object mới tạo
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}"); // ✅ Trả lỗi rõ ràng hơn
            }
        }


        // 4. PUT: Cập nhật giao dịch
        // URL: /api/GiaoDich/{id}
        [HttpPut("by-id-update/{id}")]
        public async Task<IActionResult> CapNhatGiaoDich(int id, [FromBody] thongTinUpdate dto)
        {
            try
            {
                Console.WriteLine($"❌ ID = {id} và số tiền {dto.SoTien} va mo ta {dto.MoTa}");
                if (dto.SoTien <= 0)
                    return BadRequest("Số tiền phải lớn hơn 0.");

                var giaoDich = await _context.GiaoDiches.FindAsync(id);
                if (giaoDich == null)
                {
                    Console.WriteLine($"❌ Không tìm thấy giao dịch với ID = {id} và số tiền {dto.SoTien}");
                    return NotFound("Không tìm thấy giao dịch.");
                }

                // ✅ Luôn cập nhật, không cần kiểm tra có thay đổi hay không
                giaoDich.SoTien = dto.SoTien;
                giaoDich.MoTa = dto.MoTa;

                await _context.SaveChangesAsync();
                return Ok(giaoDich); // ✅ Trả về giao dịch đã cập nhật
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }



        //5. DELETE: Xóa giao dịch
        //URL: /api/GiaoDich/{id}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiaoDich(int id)
        {
            var giaoDich = await _context.GiaoDiches.FindAsync(id);
            if (giaoDich == null)
            {
                return NotFound(new { message = "Không tìm thấy giao dịch để xóa." });
            }

            _context.GiaoDiches.Remove(giaoDich);
            await _context.SaveChangesAsync();

            return Ok(); // Trả về 204 No Content nếu xóa thành công
        }

        private async Task<bool> GiaoDichExists(int id)
        {
            return await _context.GiaoDiches.AnyAsync(e => e.Id == id);
        }
    }
}

