using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    public partial class ThanhToan : Form
    {
        public ThanhToan(UC_DanhSachDonHang dsdh)
        {
            InitializeComponent();
            txtTienThu.TextChanged += txtTienThu_TextChanged;
            btnXacNhan.Click += btnXacNhan_Click;
            uc_DSDH = dsdh;

        }
        private void txtTienThu_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTienThu.Text))
            {
                decimal tongTien = decimal.Parse(txtTongTien.Text);
                decimal tienThu = decimal.Parse(txtTienThu.Text);

                decimal tienTra = tienThu - tongTien;
                txtTienTraLai.Text = tienTra.ToString();
            }
            else
            {
                txtTienTraLai.Text = string.Empty;
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public decimal TongTien
        {
            get { return decimal.Parse(txtTongTien.Text); }
            set { txtTongTien.Text = value.ToString(); }
        }
        private UC_DanhSachDonHang uc_DSDH;
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (uc_DSDH != null)
            {
                int maDH = uc_DSDH.MaDonHang;

                uc_DSDH.CapNhatTrangThaiThanhToan(maDH);
            }

            this.Close();
        }
    }
}
