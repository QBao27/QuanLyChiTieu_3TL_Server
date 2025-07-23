namespace QuanLyChiTieu_3TL.Models
{
    public class GiaoDichDTO
    {
        public int Id { get; set; }
        public decimal SoTien { get; set; }
        public string LoaiGiaoDich { get; set; }
        public string NgayGiaoDich { get; set; }
        public string MoTa { get; set; }
        public int IdDanhMuc { get; set; }
        public int IdTaiKhoan { get; set; }
        public string Color { get; set; }
        public string TenDanhMuc { get; set; }
        public string Icon { get; set; }
    }
}
