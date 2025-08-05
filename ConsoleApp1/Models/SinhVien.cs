namespace ConsoleApp1.Models
{
    public class SinhVien
    {
        public string MaSo { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public LopHoc Lop { get; set; }

        public override string ToString()
        {
            return $"{MaSo,-10} | {HoTen,-20} | {NgaySinh:dd/MM/yyyy} | {DiaChi,-15} | {Lop?.TenLop}";
        }
    }
}
