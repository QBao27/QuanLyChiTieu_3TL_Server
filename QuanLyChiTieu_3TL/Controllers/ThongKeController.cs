using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyChiTieu_3TL.Data;
using QuanLyChiTieu_3TL.Models;


namespace QuanLyChiTieu_3TL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThongKeController : Controller
    {
        private readonly QuanLyChiTieu3tlContext _context;

        public ThongKeController(QuanLyChiTieu3tlContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet("by-tai-khoan/{idTaiKhoan}")]
        //public async Task<IActionResult> GetByTaiKhoan(int idTaiKhoan)
        //{
        //    var giaoDiches = await _context.GiaoDiches
        //        .Where(g => g.IdTaiKhoan == idTaiKhoan)
        //        .Select(g => new {
        //            g.Id,
        //            g.SoTien,
        //            g.LoaiGiaoDich,
        //            NgayGiaoDich = g.NgayGiaoDich.ToString("yyyy-MM-dd"),
        //            g.MoTa,
        //            IdDanhMuc = g.IdDanhMuc,
        //            IdTaiKhoan = g.IdTaiKhoan
        //        })
        //        .ToListAsync(); aa

        //    return Ok(giaoDiches);
       
        //}

        [HttpGet("by-tai-khoan/{idTaiKhoan}")]
        public async Task<IActionResult> GetByTaiKhoan(int idTaiKhoan)
        {
            var giaoDichs = await _context.GiaoDiches
                .Where(g => g.IdTaiKhoan == idTaiKhoan)
                .Include(g => g.IdDanhMucNavigation) // Nếu cần tên DanhMuc
                .Select(g => new GiaoDichDTO
                {
                    Id = g.Id,
                    SoTien = g.SoTien,
                    LoaiGiaoDich = g.LoaiGiaoDich,
                    NgayGiaoDich = g.NgayGiaoDich.ToString("yyyy-MM-dd"),
                    MoTa = g.MoTa,
                    IdDanhMuc = g.IdDanhMuc,
                    IdTaiKhoan = g.IdTaiKhoan,
                    Color = g.Color,
                    TenDanhMuc = g.IdDanhMucNavigation.TenDanhMuc, // Giả sử có cột TenDanhMuc
                    Icon = g.IdDanhMucNavigation.Icon // Nếu trong DanhMuc có cột Icon
                })
                .ToListAsync();

            return Ok(giaoDichs);
        }

    }
}
