using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    public partial class UC_SP_thongtinsp : UserControl
    {
        public UC_SP_thongtinsp()
        {
            InitializeComponent();
        }
        private string ma;
        private string ten;
        private int sl;
        private string phanloai;
        private decimal gia;
        private Image hinhanh;

        public string MASP
        {
            get { return ma; }
            set { ma = value; lblMa.Text = value; }
        }

        public string TENSP
        {
            get { return ten; }
            set { ten = value; lblTen.Text = value; }
        }

        public int SOLUONG
        {
            get { return sl; }
            set { sl = value; lblSL.Text = value.ToString(); }
        }

        public string PHANLOAI
        {
            get { return phanloai; }
            set { phanloai = value; lblPhanLoai.Text = value; }
        }

        public decimal GIA
        {
            get { return gia; }
            set { gia = value; lblGia.Text = value.ToString(); }
        }

        public Image HINHANH
        {
            get { return hinhanh; }
            set { hinhanh = value; picSP.Image = value; }
        }

        
        public event EventHandler XoaSanPhamClicked;

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (XoaSanPhamClicked != null)
            {
                XoaSanPhamClicked(this, EventArgs.Empty);
            }
        }

        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            ThemSanPham themSanPham = new ThemSanPham();
            themSanPham.MASP = MASP;
            themSanPham.TENSP = TENSP;
            themSanPham.SOLUONG = SOLUONG;
            themSanPham.PHANLOAI = PHANLOAI;
            themSanPham.GIA = GIA;
            themSanPham.HINHANH = HINHANH;

            themSanPham.ReadOnlyMaSP = true;

            themSanPham.Show();
        }
    }
    
}
