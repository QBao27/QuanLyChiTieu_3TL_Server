using System.ComponentModel.DataAnnotations;

namespace QuanLyChiTieu_3TL.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự.")]
        public string MatKhau { get; set; } = null!;
    }
}
