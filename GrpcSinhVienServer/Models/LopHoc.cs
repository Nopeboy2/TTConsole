namespace GrpcSinhVienServer.Models
{
    public class LopHoc
    {
        public virtual string MaLop { get; set; }
        public virtual string TenLop { get; set; }
        public virtual string MonHoc { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }
        public virtual IList<SinhVien> DanhSachSinhVien { get; set; } = new List<SinhVien>();
    }
}
