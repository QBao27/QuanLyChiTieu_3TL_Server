using System;
using System.Collections.Generic;

namespace QuanLyChiTieu_3TL.Data;

public partial class GiaoDich
{
    public int Id { get; set; }

    public decimal SoTien { get; set; }

    public string LoaiGiaoDich { get; set; } = null!;

    public DateOnly NgayGiaoDich { get; set; }

    public string? MoTa { get; set; }

    public int IdDanhMuc { get; set; }

    public int IdTaiKhoan { get; set; }

    public virtual DanhMuc IdDanhMucNavigation { get; set; } = null!;

    public virtual TaiKhoan IdTaiKhoanNavigation { get; set; } = null!;
}
