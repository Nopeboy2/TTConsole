using ConsoleApp1.Models;
using ConsoleApp1.Repositories;
using ConsoleApp1.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var repository = new InMemorySinhVienRepository();
var service = new SinhVienService(repository);
while (true)
{
    Console.WriteLine("\n=== MENU ===");
    Console.WriteLine("1. Thêm sinh viên");
    Console.WriteLine("2. Hiển thị danh sách");
    Console.WriteLine("3. Sửa sinh viên");
    Console.WriteLine("4. Xóa sinh viên");
    Console.WriteLine("5. Tìm kiếm sinh viên");
    Console.WriteLine("6. Sắp xếp theo tên");
    Console.WriteLine("0. Thoát");
    Console.Write("Chọn: ");
    string chon = Console.ReadLine();

    switch (chon)
    {
        case "1":
            var sv = NhapSinhVien();
            service.Them(sv);
            break;
        case "2":
            service.HienThi();
            break;
        case "3":
            Console.Write("Nhập mã cần sửa: ");
            var maSua = Console.ReadLine();
            var svMoi = NhapSinhVien();
            service.Sua(maSua, svMoi);
            break;
        case "4":
            Console.Write("Nhập mã cần xóa: ");
            var maXoa = Console.ReadLine();
            service.Xoa(maXoa);
            break;
        case "5":
            Console.Write("Nhập mã cần tìm: ");
            var maTim = Console.ReadLine();
            service.TimKiem(maTim);
            break;
        case "6":
            service.SapXepTheoTen();
            Console.WriteLine("Đã sắp xếp theo tên.");
            break;
        case "0":
            return;
    }
}

SinhVien NhapSinhVien()
{
    Console.Write("Mã số: ");
    string ma = Console.ReadLine();
    Console.Write("Họ tên: ");
    string ten = Console.ReadLine();
    Console.Write("Ngày sinh (dd/MM/yyyy): ");
    DateTime ns = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
    Console.Write("Địa chỉ: ");
    string dc = Console.ReadLine();

    // Lớp
    var lop = new LopHoc();
    Console.Write("Mã lớp: ");
    lop.MaLop = Console.ReadLine();
    Console.Write("Tên lớp: ");
    lop.TenLop = Console.ReadLine();
    Console.Write("Môn học: ");
    lop.MonHoc = Console.ReadLine();

    // GV
    var gv = new GiaoVien();
    Console.Write("Mã GV: ");
    gv.MaGV = Console.ReadLine();
    Console.Write("Tên GV: ");
    gv.HoTen = Console.ReadLine();
    Console.Write("Ngày sinh GV (dd/MM/yyyy): ");
    gv.NgaySinh = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);

    lop.GiaoVien = gv;

    return new SinhVien
    {
        MaSo = ma,
        HoTen = ten,
        NgaySinh = ns,
        DiaChi = dc,
        Lop = lop
    };
}
