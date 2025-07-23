namespace QuanLyChiTieu_3TL.Models
{
    public class thongTinGiaoDich
    {
        public int Id { get; set; }
        public decimal SoTien { get; set; }
        public string LoaiGiaoDich { get; set; } = null!;
        public DateOnly NgayGiaoDich { get; set; }
        public string? MoTa { get; set; }
        public int IdDanhMuc { get; set; }
        public int IdTaiKhoan { get; set; }
        public string? Color { get; set; }
    }
}
