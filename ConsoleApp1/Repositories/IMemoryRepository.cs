using ConsoleApp1.Models;
using ConsoleApp1.Repositories;
using ConsoleApp1.Models;

namespace ConsoleApp1.Repositories
{
    public class InMemorySinhVienRepository : ISinhVienRepository
    {
        private List<SinhVien> danhSach = new();

        public void Them(SinhVien sv)
        {
            danhSach.Add(sv);
        }

        public void Xoa(string maSo)
        {
            danhSach.RemoveAll(s => s.MaSo == maSo);
        }

        public void Sua(string maSo, SinhVien svMoi)
        {
            var sv = danhSach.FirstOrDefault(s => s.MaSo == maSo);
            if (sv != null)
            {
                sv.HoTen = svMoi.HoTen;
                sv.NgaySinh = svMoi.NgaySinh;
                sv.DiaChi = svMoi.DiaChi;
                sv.Lop = svMoi.Lop;
            }
        }

        public SinhVien TimKiem(string maSo)
        {
            return danhSach.FirstOrDefault(s => s.MaSo == maSo);
        }

        public List<SinhVien> LayTatCa()
        {
            return danhSach;
        }

        public void SapXepTheoTen()
        {
            danhSach = danhSach.OrderBy(s => s.HoTen).ToList();
        }
    }
}
