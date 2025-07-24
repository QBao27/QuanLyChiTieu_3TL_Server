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
    }
}

        
  

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] QuanLyChiTieu_3TL.DTOs.LoginRequest request)
        //{
        //    var user = _context.users.FirstOrDefault(u => u.Email == request.Email);
        //    if (user == null)
        //        return Unauthorized("Tài khoảng không tồn tại");

        //    if (user.Password != request.Password)
        //        return Unauthorized("Sai mật khẩu");

        //    return Ok("Đăng nhập thành công");
        //}

        //[HttpPost("logout")]
        //public IActionResult Logout()
        //{
        //    // Chỉ đơn giản phản hồi thành công, client sẽ xóa token ở phía Flutter
        //    return Ok(new { message = "Đăng xuất thành công" });
        //}
   