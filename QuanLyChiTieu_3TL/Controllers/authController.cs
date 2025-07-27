using Microsoft.AspNetCore.Mvc;
using QuanLyChiTieu_3TL.Data;
using QuanLyChiTieu_3TL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Text;
using System.Security.Cryptography;

namespace TestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly QuanLyChiTieu3tlContext _context;

        public AuthController(QuanLyChiTieu3tlContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] QuanLyChiTieu_3TL.Models.LoginRequest req)
        {
            var user = await _context.TaiKhoans
                .SingleOrDefaultAsync(u => u.Email == req.EmailOrPhone || u.Phone == req.EmailOrPhone);

            if (user == null)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            bool isValid = BCrypt.Net.BCrypt.Verify(req.MatKhau, user.MatKhau);
            if (!isValid)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            return Ok(new
            {
                id = user.Id,
                hoTen = user.HoTen,
                email = user.Email,
                phone = user.Phone
            });
        }



        // (Giữ nguyên) Hàm hash SHA256
        private string HashPassword(string plain)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
            return Convert.ToHexString(bytes);
        }

        //API đổi mật khẩu
        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest req)
        {
            var user = await _context.TaiKhoans.FindAsync(id);
            if (user == null)
            {
                Console.WriteLine($"❌ Không tìm thấy người dùng với ID: {id}");
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }

            // So sánh mật khẩu cũ
            bool isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(req.OldPassword, user.MatKhau);
            if (!isOldPasswordCorrect)
            {
                Console.WriteLine($"❌ Mật khẩu cũ không đúng cho ID: {id}");
                return BadRequest(new { message = "Mật khẩu cũ không đúng." });
            }

            // Hash mật khẩu mới
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            Console.WriteLine($"🔐 Đổi mật khẩu cho ID: {id}");
            Console.WriteLine($"👉 Mật khẩu mới (gốc): {req.NewPassword}");
            Console.WriteLine($"✅ Mật khẩu sau khi hash: {hashedPassword}");

            user.MatKhau = hashedPassword;
            await _context.SaveChangesAsync();

            Console.WriteLine("✅ Đổi mật khẩu thành công!");

            return Ok(new { message = "Đổi mật khẩu thành công." });
        }
    }

}


       
   