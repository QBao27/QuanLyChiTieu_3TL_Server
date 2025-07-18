using System;
using System.Collections.Generic;

namespace QuanLyChiTieu_3TL.Data;

public partial class DanhMuc
{
    public int Id { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? Icon { get; set; }

    public string LoaiDanhMuc { get; set; } = null!;

    public virtual ICollection<GiaoDich> GiaoDiches { get; set; } = new List<GiaoDich>();
}
