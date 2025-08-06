namespace GrpcSinhVienServer.Models
{
    public class SinhVien
    {
        public virtual string MaSo { get; set; }
        public virtual string HoTen { get; set; }
        public virtual DateTime NgaySinh { get; set; }
        public virtual string DiaChi { get; set; } 
        public virtual LopHoc Lop { get; set; }
    }
}
