using Microsoft.AspNetCore.Mvc;
using QuanLyChiTieu_3TL.Data;
using QuanLyChiTieu_3TL.Models;
using Microsoft.EntityFrameworkCore;
using QuanLyChiTieu_3TL.DTOs;
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
            // 1. Tìm user theo email hoặc số điện thoại
            var user = await _context.TaiKhoans
                .SingleOrDefaultAsync(u => u.Email == req.Email || u.Phone == req.Email);

            if (user == null)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            // 2. Hash mật khẩu client gửi lên
            var hashed = HashPassword(req.MatKhau);
            if (user.MatKhau != hashed)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            // 3. Trả về thông tin
            return Ok(new
            {
                id = user.Id,
                hoTen = user.HoTen,
                email = user.Email,
                phone = user.Phone
                // Có thể thêm token nếu cần: token = GenerateJwt(user)
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
   