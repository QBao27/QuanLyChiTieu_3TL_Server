using System;
using System.Collections.Generic;

namespace QuanLyChiTieu_3TL.Data;

public partial class TaiKhoan
{
    public int Id { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string MatKhau { get; set; } = null!;

    public virtual ICollection<GiaoDich> GiaoDiches { get; set; } = new List<GiaoDich>();
}
