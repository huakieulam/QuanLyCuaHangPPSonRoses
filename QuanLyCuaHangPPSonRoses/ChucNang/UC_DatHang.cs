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
using System.Xml.Linq;

namespace QuanLyCuaHangPPSonRoses.ChucNang
{
    public partial class UC_DatHang : UserControl
    {
        public UC_DatHang()
        {
            InitializeComponent();
        }
        string connectionString = @"Server=localhost;Port=5432;Username=postgres;Password=123;Database=postgres";


        private void UC_DatHang_Load(object sender, EventArgs e)
        {
            HienThiSP();
        }

        private Image HinhAnhMangByte(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            using (var stream = new System.IO.MemoryStream(byteArray))
            {
                return Image.FromStream(stream);
            }
        }
        List<UC_SP_thongtinsp> danhSachSanPham;
        public void HienThiSP()
        {
            flowLayoutPanel1.Controls.Clear();

            string query = "SELECT * FROM sanpham";
            danhSachSanPham = new List<UC_SP_thongtinsp>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UC_SP_thongtinsp uc_SP_Thongtinsp = new UC_SP_thongtinsp();
                            uc_SP_Thongtinsp.MASP = reader["masp"].ToString();
                            uc_SP_Thongtinsp.TENSP = reader["tensp"].ToString();
                            uc_SP_Thongtinsp.SOLUONG = Convert.ToInt32(reader["sl"]);
                            uc_SP_Thongtinsp.PHANLOAI = reader["loaisp"].ToString();
                            uc_SP_Thongtinsp.GIA = Convert.ToDecimal(reader["gia"]);
                            uc_SP_Thongtinsp.HINHANH = HinhAnhMangByte((byte[])reader["hinhanh"]);

                            danhSachSanPham.Add(uc_SP_Thongtinsp);
                        }
                    }
                }
            }

            foreach (UC_SP_thongtinsp sanPham in danhSachSanPham)
            {
                sanPham.XoaSanPhamClicked += XoaSanPham_Clicked;
                sanPham.ChinhSuaSanPhamClicked += ChinhSuaSanPham_Clicked;
                flowLayoutPanel1.Controls.Add(sanPham);
            }
        }
        private void ChinhSuaSanPham_Clicked(object sender, EventArgs e)
        {
            UC_SP_thongtinsp chonSP = (UC_SP_thongtinsp)sender;

            string maSP = chonSP.MASP;
            string tenSP = chonSP.TENSP;
            int soLuong = chonSP.SOLUONG;
            decimal gia = chonSP.GIA;
            string phanLoai = chonSP.PHANLOAI;


            bool tontaiSP = false;

            foreach (DataGridViewRow row in dgvTaoDonHang.Rows)
            {
                if (row.Cells["dgvMaSP"].Value.ToString() == maSP)
                {
                    int slHienTai = Convert.ToInt32(row.Cells["dgvSL"].Value);
                    if (soLuong > 0)
                    {
                        row.Cells["dgvSL"].Value = slHienTai + 1;

                        decimal donGia = Convert.ToDecimal(row.Cells["dgvGia"].Value);
                        decimal tongTienSP = (slHienTai + 1) * donGia;
                        row.Cells["dgvTong"].Value = tongTienSP;
                    }
                    

                    tontaiSP = true;
                    break;
                }
            }

            if (!tontaiSP)
            {
                dgvTaoDonHang.Rows.Add(dgvTaoDonHang.Rows.Count + 1, maSP, tenSP, phanLoai, 1, gia, gia);
            }
            if (soLuong > 0)
            {
                chonSP.SOLUONG = soLuong - 1;
            }
            TinhTongDonHang();
        }

        private void XoaSanPham_Clicked(object sender, EventArgs e)
        {
            UC_SP_thongtinsp sanPham = (UC_SP_thongtinsp)sender;
            string maSP = sanPham.MASP;

            foreach (DataGridViewRow row in dgvTaoDonHang.Rows)
            {
                if (row.Cells["dgvMaSP"].Value.ToString() == maSP)
                {
                    int slHienTai = Convert.ToInt32(row.Cells["dgvSL"].Value);
                    dgvTaoDonHang.Rows.Remove(row);

                    sanPham.SOLUONG += slHienTai;

                    break;
                }
            }
            TinhTongDonHang();
        }
        private void TinhTongDonHang()
        {
            decimal tongDonHang = 0;

            foreach (DataGridViewRow row in dgvTaoDonHang.Rows)
            {
                decimal giaTriDonHang = Convert.ToDecimal(row.Cells["dgvTong"].Value);
                tongDonHang += giaTriDonHang;
            }

            lblTongDonHang.Text = tongDonHang.ToString();
        }

        private void btnDanhSach_Click(object sender, EventArgs e)
        {
            DanhSachDonHang danhSachDonHang = new DanhSachDonHang();
            danhSachDonHang.Show();
        }
        private void btnTaoDonHang_Click(object sender, EventArgs e)
        {
            LuuThongTinDonHang();
            
            dgvTaoDonHang.Rows.Clear();
            txtTenKH.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            lblTongDonHang.Text = "";

            MessageBox.Show("Tạo đơn hàng thành công!");
        }
        private void LuuThongTinDonHang()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO donhang (tenKH, email, sdt, ngay, tongDH, trangthai) VALUES (@tenKH, @email, @sdt, @ngay, @tongDH, @trangthai)";
                string trangthai = "Chưa thanh toán";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tenKH", txtTenKH.Text);
                    command.Parameters.AddWithValue("@email", txtEmail.Text);
                    command.Parameters.AddWithValue("@sdt", txtSDT.Text);
                    command.Parameters.AddWithValue("@ngay", DateTime.Now);
                    command.Parameters.AddWithValue("@tongDH", Convert.ToDecimal(lblTongDonHang.Text));
                    command.Parameters.AddWithValue("@trangthai", trangthai);
                    command.ExecuteNonQuery();
                }

                string queryMaDH = "SELECT lastval()";
                int maDonHang;
                using (NpgsqlCommand command = new NpgsqlCommand(queryMaDH, connection))
                {
                    maDonHang = Convert.ToInt32(command.ExecuteScalar());
                }

                query = "INSERT INTO chitietdonhang (madh, masp, tensp, phanloai, slmua, giasp, tonggiasp) VALUES (@madh, @masp, @tensp, @phanloai, @slmua, @giasp, @tonggiasp)";
                foreach (DataGridViewRow row in dgvTaoDonHang.Rows)
                {
                    string maSP = row.Cells["dgvMaSP"].Value.ToString();
                    string tenSP = row.Cells["dgvTenSP"].Value.ToString();
                    string phanLoai = row.Cells["dgvPhanLoai"].Value.ToString();
                    int soLuongMua = Convert.ToInt32(row.Cells["dgvSL"].Value);
                    decimal giaSP = Convert.ToInt64(row.Cells["dgvGia"].Value);
                    decimal tonggiasp = Convert.ToInt64(row.Cells["dgvTong"].Value);

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@madh", maDonHang);
                        command.Parameters.AddWithValue("@masp", maSP);
                        command.Parameters.AddWithValue("@tensp", tenSP);
                        command.Parameters.AddWithValue("@phanloai", phanLoai);
                        command.Parameters.AddWithValue("@slmua", soLuongMua);
                        command.Parameters.AddWithValue("@giasp", giaSP);
                        command.Parameters.AddWithValue("@tonggiasp", tonggiasp);

                        command.ExecuteNonQuery();
                        
                    }
                }
            }
            

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            foreach (UC_SP_thongtinsp sanPham in danhSachSanPham)
            {
                if (sanPham.TENSP.ToLower().Contains(tuKhoa) || sanPham.MASP.ToLower().Contains(tuKhoa))
                {
                    sanPham.Visible = true;
                }
                else
                {
                    sanPham.Visible = false;
                }
            }
        }

        private void btnLamMoiDL_Click(object sender, EventArgs e)
        {
            dgvTaoDonHang.Rows.Clear();
            txtTenKH.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            lblTongDonHang.Text = "";

            HienThiSP();
        }
    }
}
