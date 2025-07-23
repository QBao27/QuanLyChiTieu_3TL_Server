namespace QuanLyChiTieu_3TL.Models
{
    public class user
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string HoTen { get; set; } = null!;
        public string? Phone { get; set; }
    }
}
