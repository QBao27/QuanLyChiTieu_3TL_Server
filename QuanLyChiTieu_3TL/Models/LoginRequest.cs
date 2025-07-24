using QuanLyChiTieu_3TL.DTOs;
using System.ComponentModel.DataAnnotations;

namespace QuanLyChiTieu_3TL.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email/Số điện thoại không được để trống.")]
        [EmailOrPhone(ErrorMessage = "Chuỗi phải là email hoặc số điện thoại hợp lệ.")]
        public string EmailOrPhone { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự.")]
        public string MatKhau { get; set; } = null!;
    }
}
