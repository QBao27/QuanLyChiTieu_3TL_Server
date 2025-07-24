using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace QuanLyChiTieu_3TL.DTOs
{
    public class EmailOrPhoneAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string input = value as string ?? string.Empty;

            // Regex cho email cơ bản
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            // Regex cho số điện thoại VN (10-11 số, bắt đầu bằng 0)
            var phoneRegex = new Regex(@"^(0\d{9,10})$");

            if (emailRegex.IsMatch(input) || phoneRegex.IsMatch(input))
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult("Chuỗi phải là email hoặc số điện thoại hợp lệ.");
        }
    }
}
