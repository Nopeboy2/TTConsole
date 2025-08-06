using Grpc.Net.Client;
using GrpcSinhVienClient.Protos;
using Google.Protobuf.WellKnownTypes;

namespace GrpcSinhVienClient.Services
{
    public class SinhVienClientService
    {
        private readonly SinhVienService.SinhVienServiceClient _client;

        public SinhVienClientService(SinhVienService.SinhVienServiceClient client)
        {
            _client = client;
        }

        public void XemDanhSach()
        {
            var result = _client.LayTatCa(new Empty());
            foreach (var sv in result.DanhSach)
            {
                Console.WriteLine($"{sv.MaSo} - {sv.HoTen} - {sv.NgaySinh} - {sv.DiaChi} - {sv.TenLop} - GV: {sv.TenGiaoVien}");
            }
        }

        public void ThemSinhVien()
        {
            Console.Write("Mã số: ");
            var maSo = Console.ReadLine();
            Console.Write("Họ tên: ");
            var hoTen = Console.ReadLine();
            Console.Write("Ngày sinh (yyyy-MM-dd): ");
            var ngaySinh = Console.ReadLine();
            Console.Write("Địa chỉ: ");
            var diaChi = Console.ReadLine();
            Console.Write("Tên lớp: ");
            var tenLop = Console.ReadLine();
            Console.Write("Tên giáo viên: ");
            var tenGv = Console.ReadLine();
            Console.Write("Nhập môn học: ");
            var monHoc = Console.ReadLine();
            Console.Write("Nhập ngày sinh GV (yyyy-MM-dd): ");
            var ngaySinhGV = Console.ReadLine();

            var sv = new SinhVienModel
            {
                MaSo = maSo,
                HoTen = hoTen,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                TenLop = tenLop,
                TenGiaoVien = tenGv,
                MonHoc = monHoc,
                NgaySinhGV = ngaySinhGV
            };

            _client.Them(sv);
            Console.WriteLine("✔️ Thêm thành công!");
        }

        public void SuaSinhVien()
        {
            Console.Write("Nhập mã số sinh viên cần sửa: ");
            var maSo = Console.ReadLine();

            Console.Write("Họ tên mới: ");
            var hoTen = Console.ReadLine();
            Console.Write("Ngày sinh mới (yyyy-MM-dd): ");
            var ngaySinh = Console.ReadLine();
            Console.Write("Địa chỉ mới: ");
            var diaChi = Console.ReadLine();
            Console.Write("Tên lớp mới: ");
            var tenLop = Console.ReadLine();
            Console.Write("Tên giáo viên mới: ");
            var tenGv = Console.ReadLine();

            var sv = new SinhVienModel
            {
                MaSo = maSo,
                HoTen = hoTen,
                NgaySinh = ngaySinh,
                DiaChi = diaChi,
                TenLop = tenLop,
                TenGiaoVien = tenGv
            };

            _client.Sua(sv);
            Console.WriteLine("✔️ Sửa thành công!");
        }

        public void XoaSinhVien()
        {
            Console.Write("Nhập mã số sinh viên cần xóa: ");
            var maSo = Console.ReadLine();

            var request = new SinhVienRequest { MaSo = maSo };
            _client.Xoa(request);
            Console.WriteLine("✔️ Đã xóa sinh viên.");
        }

        public void SapXepTheoTen()
        {
            var result = _client.SapXepTheoTen(new Empty());
            foreach (var sv in result.DanhSach)
            {
                Console.WriteLine($"{sv.HoTen} - {sv.MaSo}");
            }
        }

        public void TimKiem()
        {
            Console.Write("Nhập mã số sinh viên cần tìm: ");
            var maSo = Console.ReadLine();

            var result = _client.TimKiem(new SinhVienRequest { MaSo = maSo });
            if (!string.IsNullOrEmpty(result.MaSo))
            {
                Console.WriteLine($"{result.MaSo} - {result.HoTen} - {result.NgaySinh} - {result.DiaChi}");
            }
            else
            {
                Console.WriteLine("❌ Không tìm thấy.");
            }
        }
    }
}
