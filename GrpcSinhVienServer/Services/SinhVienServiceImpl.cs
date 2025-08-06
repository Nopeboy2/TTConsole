using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcSinhVienServer.DataAccess;
using GrpcSinhVienServer.Models;
using NHibernate;
using GrpcSinhVien;

namespace GrpcSinhVienServer.Services
{
    public class SinhVienServiceImpl : SinhVienService.SinhVienServiceBase
    {
        public override Task<DanhSachSinhVien> LayTatCa(Empty request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();
            var svList = session.Query<SinhVien>().ToList();

            var danhSach = new DanhSachSinhVien();
            foreach (var sv in svList)
            {
                danhSach.DanhSach.Add(new SinhVienModel
                {
                    MaSo = sv.MaSo,
                    HoTen = sv.HoTen,
                    NgaySinh = sv.NgaySinh.ToString("yyyy-MM-dd"),
                    DiaChi = sv.DiaChi,
                    TenLop = sv.Lop?.TenLop ?? "",
                    TenGiaoVien = sv.Lop?.GiaoVien?.HoTen ?? ""
                });
            }

            return Task.FromResult(danhSach);
        }

        public override Task<Empty> Them(SinhVienModel request, ServerCallContext context)
        {
            using var session = NHibernateHelper.OpenSession();
            using var tx = session.BeginTransaction();

            var gv = new GiaoVien
            {
                MaGV = "GV_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                HoTen = request.TenGiaoVien,
                NgaySinh = !string.IsNullOrWhiteSpace(request.NgaySinhGV)
                ? DateTime.Parse(request.NgaySinhGV)
                : DateTime.Now
            };

            var lop = new LopHoc
            {
                MaLop = "LH_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                TenLop = request.TenLop,
                MonHoc = request.MonHoc,                      // 👈 Dùng môn học thật
                GiaoVien = gv
            };

            var sv = new SinhVien
            {
                MaSo = request.MaSo,
                HoTen = request.HoTen,
                NgaySinh = DateTime.Parse(request.NgaySinh),
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
                sv.NgaySinh = DateTime.Parse(request.NgaySinh);
                sv.DiaChi = request.DiaChi;
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
    }
}
