using DevExpress.Utils.Gesture;
using DevExpress.XtraEditors.Mask.Design;
using DevExpress.XtraReports.UI;
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

        private string masp;
        private string tensp;
        private string loai;
        private int sl;
        private decimal gia;
        private decimal tong;
        private decimal tongGia;
        private string trangThai;
        public string MASP
        {
            get { return masp; }
            set {
                masp = value;
                if (dgvDSSPDonHang.Rows.Count > 0)
                    dgvDSSPDonHang.Rows[0].Cells["dgvMaSP"].Value = value;
            }
        }

        public string TENSP
        {
            get { return tensp; }
            set { tensp = value; 
                if(dgvDSSPDonHang.Rows.Count > 0)
                dgvDSSPDonHang.Rows[0].Cells["dgvTenSP"].Value = value; }
        }

        public string LOAI
        {
            get { return loai; }
            set { loai = value; 
                if(dgvDSSPDonHang.Rows.Count > 0)
                    dgvDSSPDonHang.Rows[0].Cells["dgvPhanLoai"].Value = value; }
        }

        public int SL
        {
            get { return sl; }
            set { sl = value; 
                if(dgvDSSPDonHang.Rows.Count > 0)
                    dgvDSSPDonHang.Rows[0].Cells["dgvSL"].Value = value; }
        }

        public decimal GIA
        {
            get { return gia; }
            set { gia = value; 
                if(dgvDSSPDonHang.Rows.Count > 0)
                    dgvDSSPDonHang.Rows[0].Cells["dgvGia"].Value = value; }
        }

        public decimal TONG
        {
            get { return tong; }
            set { tong = value; 
                if(dgvDSSPDonHang.Rows.Count > 0)
                    dgvDSSPDonHang.Rows[0].Cells["dgvTong"].Value = value; }
        }

        public decimal TONGGIA
        {
            get { return tongGia; }
            set {
                tongGia = value;
                lblTongDonHang.Text = value.ToString();
            }
        }

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
        public int MaDonHang { get; set; }
        public string TRANGTHAI
        {
            get { return trangThai; }
            set { trangThai = value; btnThanhToan.Text = value; }
        }
        private void btnChinhSua_Click(object sender, EventArgs e)
        {

        }

        private void UC_DanhSachDonHang_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres";
            string query = "SELECT trangthai FROM donhang WHERE maDH = @maDH";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@maDH", MaDonHang);
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    string trangThai = result.ToString();

                    if (trangThai == "Đã thanh toán")
                    {
                        btnThanhToan.Visible = false;
                    }
                    else
                    {
                        btnThanhToan.Visible = true;
                    }
                }

                connection.Close();
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
                ThanhToan thanhToan = new ThanhToan(this);
                thanhToan.TongTien = TONGGIA;
                thanhToan.Show();
            

        }
        public void CapNhatTrangThaiThanhToan(int maDH)
        {
            btnThanhToan.Text = "Đã thanh toán";
            btnThanhToan.Enabled = false;
            btnThanhToan.Visible = true;
            string connectionString = "Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres";
            string query = "UPDATE donhang SET trangthai = 'Đã thanh toán' WHERE maDH = @maDH";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@maDH", maDH);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            
        }
    }
}
