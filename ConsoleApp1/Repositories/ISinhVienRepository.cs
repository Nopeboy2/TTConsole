using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public interface ISinhVienRepository
    {
        void Them(SinhVien sv);
        void Xoa(string maSo);
        void Sua(string maSo, SinhVien svMoi);
        SinhVien TimKiem(string maSo);
        List<SinhVien> LayTatCa();
        void SapXepTheoTen();
    }
}
