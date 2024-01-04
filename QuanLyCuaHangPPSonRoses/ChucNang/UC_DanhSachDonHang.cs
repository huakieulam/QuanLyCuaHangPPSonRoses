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
    public partial class UC_DanhSachDonHang : UserControl
    {
        public UC_DanhSachDonHang()
        {
            InitializeComponent();
        }
        private string tenKH;
        private string sdt;
        private string email;
        public string MASP { get; set; }
        public string TENSP { get; set; }
        public string LOAI { get; set; }
        public int SL { get; set; }
        public decimal GIASP { get; set; }
        public decimal TONGDON { get; set; }


        public string TENKH
        {
            get { return tenKH; }
            set { tenKH = value; lblTenKH.Text = value; }
        }

        public string SDT
        {
            get { return sdt; }
            set { sdt = value; lblSDT.Text = value; }
        }

       
        public string EMAIL
        {
            get { return email; }
            set { email = value; lblEmail.Text = value; }
        }

        
        private void btnChinhSua_Click(object sender, EventArgs e)
        {

        }

        private void UC_DanhSachDonHang_Load(object sender, EventArgs e)
        {
        }
    }
}
