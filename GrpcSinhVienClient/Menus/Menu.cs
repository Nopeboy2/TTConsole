using GrpcSinhVienClient.Services;

namespace GrpcSinhVienClient.Menus
{
    public class Menu
    {
        private readonly SinhVienClientService _service;

        public Menu(SinhVienClientService service)
        {
            _service = service;
        }

        public void HienThi()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== QUẢN LÝ SINH VIÊN ====");
                Console.WriteLine("1. Xem danh sách sinh viên");
                Console.WriteLine("2. Thêm sinh viên");
                Console.WriteLine("3. Sửa sinh viên");
                Console.WriteLine("4. Xóa sinh viên");
                Console.WriteLine("5. Sắp xếp theo tên");
                Console.WriteLine("6. Tìm kiếm theo mã số");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");

                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        _service.XemDanhSach();
                        break;
                    case "2":
                        _service.ThemSinhVien();
                        break;
                    case "3":
                        _service.SuaSinhVien();
                        break;
                    case "4":
                        _service.XoaSinhVien();
                        break;
                    case "5":
                        _service.SapXepTheoTen();
                        break;
                    case "6":
                        _service.TimKiem();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Chức năng không hợp lệ!");
                        break;
                }

                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
            }
        }
    }
}
