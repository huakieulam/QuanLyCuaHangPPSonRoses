using QuanLyCuaHangPPSonRoses.ChucNang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangPPSonRoses
{
    public partial class KhachHang : Form
    {
        public KhachHang()
        {
            InitializeComponent();
        }
        UC_DatHang uc_DatHang = new UC_DatHang();
        UC_TrangChu uc_TrangChu = new UC_TrangChu();
        UC_QLTaiKhoan uc_QLTaiKhoan = new UC_QLTaiKhoan();
        private void KhachHang_Load(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_TrangChu);
            uc_TrangChu.Dock = DockStyle.Fill;
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
            panel2.Controls.Add(uc_QLTaiKhoan);
            uc_QLTaiKhoan.Dock = DockStyle.Fill;
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(uc_TrangChu);
            uc_TrangChu.Dock = DockStyle.Fill;
        }
    }
}
