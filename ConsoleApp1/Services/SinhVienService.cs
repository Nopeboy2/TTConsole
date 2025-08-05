using ConsoleApp1.Models;
using ConsoleApp1.Repositories;
namespace ConsoleApp1.Services
{
    public class SinhVienService
    {
        private readonly ISinhVienRepository _repository;

        public SinhVienService(ISinhVienRepository repository)
        {
            _repository = repository;
        }

        public void Them(SinhVien sv) => _repository.Them(sv);

        public void HienThi()
        {
            Console.WriteLine("\nDANH SÁCH SINH VIÊN:");
            foreach (var sv in _repository.LayTatCa())
                Console.WriteLine(sv);
        }

        public void Xoa(string maSo) => _repository.Xoa(maSo);

        public void Sua(string maSo, SinhVien svMoi) => _repository.Sua(maSo, svMoi);

        public void TimKiem(string maSo)
        {
            var sv = _repository.TimKiem(maSo);
            Console.WriteLine(sv != null ? sv.ToString() : "Không tìm thấy.");
        }

        public void SapXepTheoTen() => _repository.SapXepTheoTen();
    }
}
