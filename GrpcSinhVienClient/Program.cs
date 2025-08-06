using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcSinhVienClient.Protos;
using System.Text;


class Program
{
    static async Task Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        using var channel = GrpcChannel.ForAddress("http://localhost:5242");
        var client = new SinhVienService.SinhVienServiceClient(channel);

        while (true)
        {
            Console.WriteLine("\n====== MENU ======");
            Console.WriteLine("1. Xem danh sách sinh viên");
            Console.WriteLine("2. Thêm mới sinh viên");
            Console.WriteLine("3. Chỉnh sửa sinh viên");
            Console.WriteLine("4. Xóa sinh viên");
            Console.WriteLine("5. Sắp xếp theo tên");
            Console.WriteLine("6. Tìm kiếm theo mã số");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn chức năng: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await XemDanhSach(client);
                    break;
                case "2":
                    await ThemMoi(client);
                    break;
                case "3":
                    await ChinhSua(client);
                    break;
                case "4":
                    await Xoa(client);
                    break;
                case "5":
                    await SapXep(client);
                    break;
                case "6":
                    await TimKiem(client);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Chức năng không hợp lệ!");
                    break;
            }
        }
    }

    static async Task XemDanhSach(SinhVienService.SinhVienServiceClient client)
    {
        var danhSach = await client.LayTatCaAsync(new Empty());
        foreach (var sv in danhSach.DanhSach)
        {
            HienThiSinhVien(sv);
        }
    }

    static async Task ThemMoi(SinhVienService.SinhVienServiceClient client)
    {
        var sv = NhapThongTinSinhVien();
        await client.ThemAsync(sv);
        Console.WriteLine("✔ Thêm thành công.");
    }

    static async Task ChinhSua(SinhVienService.SinhVienServiceClient client)
    {
        Console.Write("Nhập mã số sinh viên cần sửa: ");
        string maSo = Console.ReadLine();

        var svMoi = NhapThongTinSinhVien();
        svMoi.MaSo = maSo;

        await client.SuaAsync(svMoi);
        Console.WriteLine("✔ Cập nhật thành công.");
    }

    static async Task Xoa(SinhVienService.SinhVienServiceClient client)
    {
        Console.Write("Nhập mã số sinh viên cần xóa: ");
        string maSo = Console.ReadLine();

        await client.XoaAsync(new SinhVienRequest { MaSo = maSo });
        Console.WriteLine("✔ Xóa thành công.");
    }

    static async Task SapXep(SinhVienService.SinhVienServiceClient client)
    {
        var ketQua = await client.SapXepTheoTenAsync(new Empty());
        foreach (var sv in ketQua.DanhSach)
        {
            HienThiSinhVien(sv);
        }
    }

    static async Task TimKiem(SinhVienService.SinhVienServiceClient client)
    {
        Console.Write("Nhập mã số sinh viên cần tìm: ");
        string maSo = Console.ReadLine();

        var sv = await client.TimKiemAsync(new SinhVienRequest { MaSo = maSo });
        if (!string.IsNullOrWhiteSpace(sv.MaSo))
            HienThiSinhVien(sv);
        else
            Console.WriteLine("❌ Không tìm thấy sinh viên.");
    }

    static SinhVienModel NhapThongTinSinhVien()
    {
        Console.Write("Họ tên: ");
        string hoTen = Console.ReadLine();
        Console.Write("Ngày sinh (yyyy-MM-dd): ");
        string ngaySinh = Console.ReadLine();
        Console.Write("Địa chỉ: ");
        string diaChi = Console.ReadLine();
        Console.Write("Tên lớp: ");
        string tenLop = Console.ReadLine();
        Console.Write("Tên giáo viên: ");
        string tenGV = Console.ReadLine();
        Console.Write("Mã số: ");
        string maSo = Console.ReadLine();

        return new SinhVienModel
        {
            MaSo = maSo,
            HoTen = hoTen,
            NgaySinh = ngaySinh,
            DiaChi = diaChi,
            TenLop = tenLop,
            TenGiaoVien = tenGV
        };
    }

    static void HienThiSinhVien(SinhVienModel sv)
    {
        Console.WriteLine($"- {sv.MaSo} | {sv.HoTen} | {sv.NgaySinh} | {sv.DiaChi} | {sv.TenLop} | GV: {sv.TenGiaoVien}");
    }
}
