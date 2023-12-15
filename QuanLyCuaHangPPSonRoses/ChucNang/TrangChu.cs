using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    public partial class TrangChu : Form
    {
        UC_TrangChu uc_TrangChu = new UC_TrangChu();
        UC_SanPham uc_SanPham = new UC_SanPham();
        UC_DatHang uc_DatHang = new UC_DatHang();
        UC_ThongKe uc_Thongke = new UC_ThongKe();
        UC_QLTaiKhoan uc_QLTK = new UC_QLTaiKhoan();

        public TrangChu()
        {
            InitializeComponent();
        }
        private void TrangChu_Load(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_TrangChu);
            uc_TrangChu.Dock = DockStyle.Fill;
        }
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_TrangChu);
            uc_TrangChu.Dock = DockStyle.Fill;
        }
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_Thongke);
            uc_Thongke.Dock = DockStyle.Fill;
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_SanPham);
            uc_SanPham.Dock = DockStyle.Fill;
        }

        private void btnDatHang_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_DatHang);
            uc_DatHang.Dock = DockStyle.Fill;
        }

        private void btnQLTaiKhoan_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_QLTK);
            uc_QLTK.Dock = DockStyle.Fill;
        }
    }
}
