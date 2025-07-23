using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChiTieu_3TL.Data;
using QuanLyChiTieu_3TL.Models;
using BCrypt.Net;

namespace QuanLyChiTieu_3TL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DangKyController : Controller
    {
        private readonly QuanLyChiTieu3tlContext _context;

        public DangKyController(QuanLyChiTieu3tlContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost("dangky")]
        public async Task<IActionResult> DangKy([FromBody] TaiKhoanDto model)
        {
            var emailExists = await _context.TaiKhoans.AnyAsync(t => t.Email == model.Email);
            var phoneExists = !string.IsNullOrEmpty(model.Phone) && await _context.TaiKhoans.AnyAsync(t => t.Phone == model.Phone);

            if (emailExists)
            {
                return BadRequest(new { message = "Email đã tồn tại." });
            }

            if (phoneExists)
            {
                return BadRequest(new { message = "Số điện thoại đã được sử dụng." });
            }

            // Băm mật khẩu trước khi lưu
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.MatKhau);

            var taiKhoan = new TaiKhoan
            {
                HoTen = model.HoTen,
                Email = model.Email,
                Phone = model.Phone,
                MatKhau = hashedPassword
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công", id = taiKhoan.Id });
        }

    }
}
