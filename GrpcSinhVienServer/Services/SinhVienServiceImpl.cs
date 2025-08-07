using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcSinhVienServer.DataAccess;
using GrpcSinhVienServer.Models;
using NHibernate;
using GrpcSinhVien;
using NHibernate.Linq;

namespace GrpcSinhVienServer.Services
{
    public class SinhVienServiceImpl : SinhVienService.SinhVienServiceBase
    {
        //public override Task<DanhSachSinhVien> LayTatCa(Empty request, ServerCallContext context)
        //{
        //    using var session = NHibernateHelper.OpenSession();
        //    var svList = session.Query<SinhVien>().ToList();

        //    var danhSach = new DanhSachSinhVien();
        //    foreach (var sv in svList)
        //    {
        //        danhSach.DanhSach.Add(new SinhVienModel
        //        {
        //            MaSo = sv.MaSo,
        //            HoTen = sv.HoTen,
        //            NgaySinh = sv.NgaySinh.ToString("yyyy-MM-dd"),
        //            DiaChi = sv.DiaChi,
        //            TenLop = sv.Lop?.TenLop ?? "",
        //            TenGiaoVien = sv.Lop?.GiaoVien?.HoTen ?? ""
        //        });
        //    }

        //    return Task.FromResult(danhSach);
        //}

        //public override Task<Empty> Them(SinhVienModel request, ServerCallContext context)
        //{
        //    using var session = NHibernateHelper.OpenSession();
        //    using var tx = session.BeginTransaction();

        //    var gv = new GiaoVien
        //    {
        //        MaGV = "GV_" + Guid.NewGuid().ToString("N").Substring(0, 6),
        //        HoTen = request.TenGiaoVien,
        //        NgaySinh = !string.IsNullOrWhiteSpace(request.NgaySinhGV)
        //        ? DateTime.Parse(request.NgaySinhGV)
        //        : DateTime.Now
        //    };

        //    var lop = new LopHoc
        //    {
        //        MaLop = "LH_" + Guid.NewGuid().ToString("N").Substring(0, 6),
        //        TenLop = request.TenLop,
        //        MonHoc = request.MonHoc,                      // 👈 Dùng môn học thật
        //        GiaoVien = gv
        //    };

        //    var sv = new SinhVien
        //    {
        //        MaSo = request.MaSo,
        //        HoTen = request.HoTen,
        //        NgaySinh = DateTime.Parse(request.NgaySinh),
        //        DiaChi = request.DiaChi,
        //        Lop = lop
        //    };

        //    session.Save(gv);
        //    session.Save(lop);
        //    session.Save(sv);

        //    tx.Commit();
        //    return Task.FromResult(new Empty());
        //}


        //public override Task<Empty> Sua(SinhVienModel request, ServerCallContext context)
        //{
        //    using var session = NHibernateHelper.OpenSession();
        //    using var tx = session.BeginTransaction();

        //    var sv = session.Get<SinhVien>(request.MaSo);
        //    if (sv != null)
        //    {
        //        sv.HoTen = request.HoTen;
        //        sv.NgaySinh = DateTime.Parse(request.NgaySinh);
        //        sv.DiaChi = request.DiaChi;
        //        session.Update(sv);
        //    }

        //    tx.Commit();
        //    return Task.FromResult(new Empty());
        //}

        //public override Task<Empty> Xoa(SinhVienRequest request, ServerCallContext context)
        //{
        //    using var session = NHibernateHelper.OpenSession();
        //    using var tx = session.BeginTransaction();

        //    var sv = session.Get<SinhVien>(request.MaSo);
        //    if (sv != null)
        //    {
        //        session.Delete(sv);
        //    }

        //    tx.Commit();
        //    return Task.FromResult(new Empty());
        //}

        //public override Task<SinhVienModel> TimKiem(SinhVienRequest request, ServerCallContext context)
        //{
        //    using var session = NHibernateHelper.OpenSession();
        //    var sv = session.Get<SinhVien>(request.MaSo);

        //    if (sv == null)
        //        return Task.FromResult(new SinhVienModel());

        //    return Task.FromResult(new SinhVienModel
        //    {
        //        MaSo = sv.MaSo,
        //        HoTen = sv.HoTen,
        //        NgaySinh = sv.NgaySinh.ToString("yyyy-MM-dd"),
        //        DiaChi = sv.DiaChi,
        //        TenLop = sv.Lop?.TenLop ?? "",
        //        TenGiaoVien = sv.Lop?.GiaoVien?.HoTen ?? ""
        //    });
        //}
        public override Task<DanhSachSinhVien> LayTatCa(Empty request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();

            // Lấy danh sách sinh viên + bao gồm các quan hệ cần thiết
            var svList = session.Query<SinhVien>()
                .Fetch(sv => sv.Lop)
                .ThenFetch(lop => lop.GiaoVien)
                .ToList();

            var danhSach = new DanhSachSinhVien();

            foreach (var sv in svList)
            {
                var giaoVien = sv.Lop?.GiaoVien;
                var lop = sv.Lop;

                danhSach.DanhSach.Add(new SinhVienModel
                {
                    MaSo = sv.MaSo,
                    HoTen = sv.HoTen,
                    NgaySinh = sv.NgaySinh.ToString("dd/MM/yyyy"),       // Format ngày sinh sinh viên
                    DiaChi = sv.DiaChi,
                    TenLop = lop?.TenLop ?? "",
                    TenGiaoVien = giaoVien?.HoTen ?? "",
                    MonHoc = lop?.MonHoc ?? "",
                    NgaySinhGV = giaoVien?.NgaySinh.ToString("dd/MM/yyyy") ?? ""
                });
            }

            return Task.FromResult(danhSach);
        }


        public override Task<Empty> Them(SinhVienModel request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();
            using var tx = session.BeginTransaction();

            var existing = session.Get<SinhVien>(request.MaSo);
            if (existing != null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Mã số sinh viên đã tồn tại"));
            }

            var gv = new GiaoVien
            {
                MaGV = "GV_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                HoTen = request.TenGiaoVien,
                NgaySinh = ParseDate(request.NgaySinhGV)
            };

            var lop = new LopHoc
            {
                MaLop = "LH_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                TenLop = request.TenLop,
                MonHoc = request.MonHoc,
                GiaoVien = gv
            };

            var sv = new SinhVien
            {
                MaSo = request.MaSo,
                HoTen = request.HoTen,
                NgaySinh = ParseDate(request.NgaySinh),
                DiaChi = request.DiaChi,
                Lop = lop
            };

            session.Save(gv);
            session.Save(lop);
            session.Save(sv);

            tx.Commit();
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Sua(SinhVienModel request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();
            using var tx = session.BeginTransaction();

            var sv = session.Get<SinhVien>(request.MaSo);
            if (sv != null)
            {
                sv.HoTen = request.HoTen;
                sv.NgaySinh = ParseDate(request.NgaySinh);
                sv.DiaChi = request.DiaChi;

                if (sv.Lop != null)
                {
                    sv.Lop.TenLop = request.TenLop;
                    sv.Lop.MonHoc = request.MonHoc;

                    if (sv.Lop.GiaoVien != null)
                    {
                        sv.Lop.GiaoVien.HoTen = request.TenGiaoVien;
                        sv.Lop.GiaoVien.NgaySinh = ParseDate(request.NgaySinhGV);
                        session.Update(sv.Lop.GiaoVien);
                    }

                    session.Update(sv.Lop);
                }

                session.Update(sv);
            }

            tx.Commit();
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Xoa(SinhVienRequest request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();
            using var tx = session.BeginTransaction();

            var sv = session.Get<SinhVien>(request.MaSo);
            if (sv != null)
            {
                session.Delete(sv);
            }

            tx.Commit();
            return Task.FromResult(new Empty());
        }

        public override Task<SinhVienModel> TimKiem(SinhVienRequest request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();
            var sv = session.Get<SinhVien>(request.MaSo);

            if (sv == null)
                return Task.FromResult(new SinhVienModel());

            return Task.FromResult(new SinhVienModel
            {
                MaSo = sv.MaSo,
                HoTen = sv.HoTen,
                NgaySinh = sv.NgaySinh.ToString("yyyy-MM-dd"),
                DiaChi = sv.DiaChi,
                TenLop = sv.Lop?.TenLop ?? "",
                TenGiaoVien = sv.Lop?.GiaoVien?.HoTen ?? ""
            });
        }

        public override Task<DanhSachSinhVien> TimKiemTheoTen(SinhVienRequest request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();
            var sinhViens = session.Query<SinhVien>()
                .Where(sv => sv.HoTen.ToLower().Contains(request.MaSo.ToLower()))
                .ToList();

            var result = new DanhSachSinhVien();
            foreach (var sv in sinhViens)
            {
                result.DanhSach.Add(new SinhVienModel
                {
                    MaSo = sv.MaSo,
                    HoTen = sv.HoTen,
                    NgaySinh = sv.NgaySinh.ToString("yyyy-MM-dd"),
                    DiaChi = sv.DiaChi,
                    TenLop = sv.Lop?.TenLop ?? "",
                    TenGiaoVien = sv.Lop?.GiaoVien?.HoTen ?? ""
                });
            }

            return Task.FromResult(result);
        }

        private DateTime ParseDate(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return DateTime.Now;

            var parts = input.Split('/', '-', '.');
            if (parts.Length == 3)
            {
                var dd = parts[0].PadLeft(2, '0');
                var mm = parts[1].PadLeft(2, '0');
                var yyyy = parts[2];
                if (DateTime.TryParse($"{yyyy}-{mm}-{dd}", out var date))
                    return date;
            }
            return DateTime.Now;
        }
    }
}
